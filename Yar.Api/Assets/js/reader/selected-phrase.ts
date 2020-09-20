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
        return Helper.getSentence(this.$element);
    }
}
