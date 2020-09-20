import * as $ from 'jquery';

export const ClassPrefix = '__';

export class Helper {
    private static sentenceMarkers = new RegExp(/[\.\?!。]/);

    public static isUndefined(value): boolean {
        return typeof value === 'undefined';
    }

    public static isEmpty($element: JQuery): boolean {
        if ($element === undefined || $element === null || !this.any($element)) {
            return true;
        }

        return false;
    }

    public static any($element: JQuery): boolean {
        return $element.length > 0;
    }

    public static isFragment($element: JQuery): boolean {
        if (Helper.isEmpty($element)) {
            return false;
        }

        return $element.hasClass(`${ClassPrefix}fragment`);
    }

    public static isInFragment($element: JQuery): boolean {
        if (Helper.isEmpty($element)) {
            return false;
        }

        return this.isFragment($element.parent());
    }

    public static getElementFragment($element: JQuery): JQuery {
        let $e: JQuery;

        if (this.isInFragment($element)) {
            $e = $element.parent(`span.${ClassPrefix}fragment`);
        } else {
            $e = $element;
        }

        return $e;
    }

    public static elementIsTerm($element: JQuery): boolean {
        return $element.hasClass(`${ClassPrefix}term`);
    }

    public static elementIsPunctuation($element: JQuery): boolean {
        return $element.hasClass(`${ClassPrefix}punctuation`);
    }

    public static elementIsSentenceMarker($element: JQuery): boolean {
        return this.sentenceMarkers.test($element.text());
    }

    public static getSentence($element: JQuery<HTMLElement>): string {
        if (Helper.isEmpty($element)) {
            return '';
        }

        let $startNode: JQuery;
        let $endNode: JQuery;

        if (Helper.isFragment($element)) {
            // TODO:
            $startNode = $element.find(`span.${ClassPrefix}term,span.${ClassPrefix}nt`).first();
            $endNode = $element.find(`span.${ClassPrefix}term,span.${ClassPrefix}nt`).last();
        } else {
            $startNode = $element;
            $endNode = $element;
        }

        const searchableNodes: JQuery = $element.parents('td.__active').find(`span.${ClassPrefix}term,span.${ClassPrefix}nt`);

        let startIndex = -1,
            endIndex = -1;

        for (let i = 0; i < searchableNodes.length; i++) {
            if (searchableNodes[i] === $startNode[0]) {
                startIndex = i;
            }

            if (searchableNodes[i] === $endNode[0]) {
                endIndex = i;
            }

            if (startIndex >= 0 && endIndex >= 0) {
                break;
            }
        }

        let sentence = '';
        const fromNode = this.backwards(searchableNodes, startIndex);
        const toNode = this.forwards(searchableNodes, endIndex);

        let counter = 0;
        let node = fromNode;

        if (Helper.elementIsPunctuation(node) && Helper.elementIsSentenceMarker(node)) {
            node = node.next();
        }

        while (node[0] !== toNode[0] && counter < 100) {
            counter++;
            sentence += node.text();
            node = node.next();
        }

        sentence += node.text();

        return sentence.replace(/\s+/g, ' ').trim();
    }

    private static forwards(searchableNodes: JQuery, index: number) {
        for (let i = index + 1; i < searchableNodes.length; i++) {
            const node = $(searchableNodes[i]);

            if (Helper.elementIsPunctuation(node) && Helper.elementIsSentenceMarker(node)) {
                return node;
            }
        }

        return $(searchableNodes[searchableNodes.length - 1]);
    }

    private static backwards(searchableNodes: JQuery, index: number) {
        for (let i = index - 1; i >= 0; i--) {
            const node = $(searchableNodes[i]);

            if (Helper.elementIsPunctuation(node) && Helper.elementIsSentenceMarker(node)) {
                return node;
            }
        }

        return $(searchableNodes[0]);
    }
}
