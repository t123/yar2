import * as $ from 'jquery';

import { SelectedPhraseData } from './selected-phrase-data';
import { Helper, ClassPrefix } from './helper';
import { ReadWordResponseModel, UndoResponseModel } from './models';

export class SelectedPhrase {
    private $container: JQuery;
    private $element: JQuery;
    private translation?: string;

    public isSameAs($otherElement: JQuery): boolean {
        if (this.$element === null || $otherElement === null) {
            return false;
        }

        return this.$element.is($otherElement);
    }

    public deselect() {
        $(`.${ClassPrefix}current`).removeClass(`${ClassPrefix}current`);
        this.$element = null;
    }

    public select($element) {
        this.deselect();

        this.$element = $element;
        this.$element.addClass(`${ClassPrefix}current`);

        if (!this.$container) {
            this.$container = this.$element.parents('table.reading-table');
        }
    }

    public getElement(): JQuery {
        return this.$element;
    }

    public setUndoResponse(response: UndoResponseModel) {
        const $elements = this.getElementsByPhrase(response.phraseLower);
        $elements.removeClass(`${ClassPrefix}known ${ClassPrefix}notknown ${ClassPrefix}notseen ${ClassPrefix}ignored`);
        $elements.attr('data-definition', '');

        if (Helper.isFragment(this.$element)) {
            this.$element.find('span').unwrap();
        }
    }

    public setTranslationResponse(response: ReadWordResponseModel) {
        this.setState(response);
        this.setTranslation(response.translation);
    }

    public setSavedResponse(response: ReadWordResponseModel) {
        this.setState(response);
        this.setTranslation(response.translation);
    }

    private setTranslation(translation: string): void {
        this.translation = translation;
    }

    private getElementsByPhrase(phraseLower: string): JQuery {
        if (Helper.isFragment(this.$element)) {
            return this.$element;
        }

        return this.$container.find(`span.--${phraseLower}`);
    }

    private setState(response: ReadWordResponseModel): void {
        const state = response.state.toLowerCase();
        const $elements = this.getElementsByPhrase(response.phraseLower);

        $elements.attr('data-definition', response.translation);

        switch (state) {
            case 'known':
            case 'notknown':
            case 'notseen':
            case 'ignored':
                $elements.removeClass(`${ClassPrefix}known ${ClassPrefix}notknown ${ClassPrefix}notseen ${ClassPrefix}ignored`).addClass(`${ClassPrefix}${state}`);
                return;
        }

        throw Error(`Unknown state: ${state}`);
    }

    public getData(): SelectedPhraseData {
        return new SelectedPhraseData(this.getPhrase(), this.getTranslation(), this.getSentence());
    }

    public getTranslation(): string {
        return this.translation;
    }

    public getPhrase(): string {
        if (Helper.isEmpty(this.$element)) {
            return '';
        }

        if (Helper.isFragment(this.$element)) {
            let text = '';
            const children = this.$element.find(`.${ClassPrefix}term,.${ClassPrefix}nt`);

            for (let i = 0; i < children.length; i++) {
                const child = $(children[i]);

                if (child[0].childNodes[0].nodeType === 3) {
                    text += child[0].childNodes[0].nodeValue || '';
                } else {
                    // text += child[0].childNodes[0].innerText;
                }
            }

            return text;
        } else {
            if (this.$element[0].childNodes[0].nodeType === 3) {
                return this.$element[0].childNodes[0].nodeValue || '';
            } else {
                // return $element[0].childNodes[0].innerText || '';
            }
        }

        return '';
    }

    public getSentence(): string {
        if (Helper.isEmpty(this.$element)) {
            return '';
        }

        let $startNode: JQuery;
        let $endNode: JQuery;

        if (Helper.isFragment(this.$element)) {
            // TODO:
            $startNode = this.$element.find(`span.${ClassPrefix}term,span.${ClassPrefix}nt`).first();
            $endNode = this.$element.find(`span.${ClassPrefix}term,span.${ClassPrefix}nt`).last();
        } else {
            $startNode = this.$element;
            $endNode = this.$element;
        }

        const searchableNodes: JQuery = this.$element.parents('td.__active').find(`span.${ClassPrefix}term,span.${ClassPrefix}nt`);

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

    private forwards(searchableNodes: JQuery, index: number) {
        for (let i = index + 1; i < searchableNodes.length; i++) {
            const node = $(searchableNodes[i]);

            if (Helper.elementIsPunctuation(node) && Helper.elementIsSentenceMarker(node)) {
                return node;
            }
        }

        return $(searchableNodes[searchableNodes.length - 1]);
    }

    private backwards(searchableNodes: JQuery, index: number) {
        for (let i = index - 1; i >= 0; i--) {
            const node = $(searchableNodes[i]);

            if (Helper.elementIsPunctuation(node) && Helper.elementIsSentenceMarker(node)) {
                return node;
            }
        }

        return $(searchableNodes[0]);
    }
}
