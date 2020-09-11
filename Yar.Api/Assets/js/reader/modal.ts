import * as $ from 'jquery';

import { Helper } from './helper';
import { ModalData } from './modal-data';
import { ReadWordResponseModel, LanguageOptionsModel } from './models';

export class Modal {
    private $attachedElement: JQuery<HTMLElement>;
    private readonly $readingContainer: JQuery<HTMLElement>;
    private isVisible: boolean;
    private isShowingMore: boolean;
    private options: LanguageOptionsModel;
    private cancelId?: string;
    private canUndo: boolean;

    private $modal: JQuery<HTMLElement>;
    private $modalTranslation: JQuery<HTMLElement>;
    private $modalPhraseBase: JQuery<HTMLElement>;
    private $modalNotes: JQuery<HTMLElement>;
    private $modalState: JQuery<HTMLElement>;
    private $modalSectionMore: JQuery<HTMLElement>;
    private $modalStats: JQuery<HTMLElement>;

    private $modalLinkSentence: JQuery<HTMLElement>;
    private $modalLinkGoogle: JQuery<HTMLElement>;
    private $modalLinkBing: JQuery<HTMLElement>;
    private $modalLinkForvo: JQuery<HTMLElement>;
    private $modalLinkCustom: JQuery<HTMLElement>;

    constructor(readingElement: HTMLElement) {
        this.$readingContainer = $(readingElement);

        this.$modal = this.$readingContainer.find('div.modal');

        this.$modalTranslation = this.$modal.find('input[name="translation"]');
        this.$modalPhraseBase = this.$modal.find('input[name="phrase-base"]');
        this.$modalNotes = this.$modal.find('textarea[name="notes"]');
        this.$modalState = this.$modal.find('input[name="state"]');
        this.$modalSectionMore = this.$modal.find('.more');
        this.$modalStats = this.$modal.find('.stats');
        this.$modalLinkSentence = this.$modal.find('.links').find('a.translate');
        this.$modalLinkGoogle = this.$modal.find('.links').find('a.google');
        this.$modalLinkBing = this.$modal.find('.links').find('a.bing');
        this.$modalLinkForvo = this.$modal.find('.links').find('a.forvo');
        this.$modalLinkCustom = this.$modal.find('.links').find('div.custom-links');
    }

    public showModal(attachedElement: JQuery<HTMLElement>, response: ReadWordResponseModel, options: LanguageOptionsModel): void {
        this.resetModal();

        this.$attachedElement = attachedElement;
        this.isVisible = true;
        this.isShowingMore = false;
        this.options = options;
        this.$modal.removeClass('hidden');

        if (response) {
            this.$modalTranslation.val(response.translation);
            this.$modalTranslation.addClass(response.state);
            this.$modalPhraseBase.val(response.phraseBase);
            this.$modalNotes.val(response.notes);
            this.$modalState.filter(`[value=${response.state}]`).prop('checked', true);
            this.canUndo = response.canUndo;
            this.cancelId = response.canUndo ? response.uuid : null;
        }

        if (this.options.popupModal !== 'popup') {
            this.toggleShowMore();
        }

        if (!this.options.hasGoogle) {
            this.$modalLinkGoogle.addClass('hidden');
        }

        if (!this.options.hasBing) {
            this.$modalLinkBing.addClass('hidden');
        }

        if (!this.options.hasGoogleTranslate) {
            this.$modalLinkSentence.addClass('hidden');
        }

        if (!this.options.hasForvo) {
            this.$modalLinkForvo.addClass('hidden');
        }

        if (!this.options.hasCustomDictionary) {
            this.$modalLinkCustom.addClass('hidden');
        } else {
            this.$modalLinkCustom.html('');

            this.options.customDictionaryUrl.forEach((url, i) => {
                this.$modalLinkCustom.append(`<a class="custom" title="Query ${url}" data-url=${url}>${i}</a>`);
            });
        }

        if (this.options.showTermStatistics) {
            const commonness = this.$attachedElement.attr('data-frequency-commonness');

            let display = '';
            display += `<span title="Occurrences in this text">${this.$attachedElement.attr('data-occurrences')}</span>/`;
            display += `<span title="Frequency of not seen words">${this.$attachedElement.attr('data-frequency-notseen')}%</span>/`;
            display += `<span title="Frequency of all words">${this.$attachedElement.attr('data-frequency-total')}%</span>`;

            if (commonness) {
                display += `/<span title="The number ${commonness} most common word in this text">${commonness}</span>`;
            }

            this.$modalStats.html(display);
        }

        this.setModalPosition();
        this.$modalTranslation.focus();
    }

    private setModalPosition() {
        if (Helper.isEmpty(this.$attachedElement)) {
            return;
        }

        const modalHeight = this.$modal.innerHeight();
        const modalWidth = this.$modal.innerWidth();

        if (this.options.centreModal && this.isShowingMore) {
            const width = this.$readingContainer.innerWidth();
            const height = this.$readingContainer.innerHeight();

            const newLeft = (width - modalWidth) / 2;
            const newTop = (height - modalHeight) / 2;

            this.$modal.css('left', newLeft);
            this.$modal.css('top', newTop);
        } else {
            const offset = this.$attachedElement.offset();
            const elementHeight = this.$attachedElement.height();
            const readingContainerOffset = this.$readingContainer.offset();
            const newTop = offset.top - readingContainerOffset.top - modalHeight - elementHeight;

            if (newTop < 0) {
                this.$modal.css('top', offset.top - readingContainerOffset.top + elementHeight + 5);
            } else {
                this.$modal.css('top', offset.top - readingContainerOffset.top - modalHeight - elementHeight);
            }

            if (this.$readingContainer.width() < offset.left - readingContainerOffset.left + modalWidth) {
                this.$modal.css('left', offset.left - readingContainerOffset.left - modalWidth);
            } else {
                this.$modal.css('left', offset.left - readingContainerOffset.left);
            }
        }
    }

    public setTranslation(translation: string) {
        if (translation) {
            this.$modalTranslation.val(translation);
        }
    }

    public hideModal(): void {
        this.isVisible = false;
        this.$modal.addClass('hidden');
        this.resetModal();
    }

    private resetModal() {
        this.$attachedElement = null;

        this.$modalTranslation.val('');
        this.$modalTranslation.removeClass();
        this.$modalNotes.val('');
        this.$modalPhraseBase.val('');
        this.$modalState.prop('checked', false);

        this.cancelId = null;

        if (this.isShowingMore) {
            this.toggleShowMore();
        }
    }

    public isModalVisible(): boolean {
        return this.isVisible;
    }

    public getModal(): JQuery<HTMLElement> {
        return this.$modal;
    }

    public toggleShowMore() {
        if (this.isShowingMore) {
            this.$modal.removeClass('expanded');
            this.$modalSectionMore.hide();
            this.isShowingMore = false;
        } else {
            this.$modal.addClass('expanded');
            this.$modalSectionMore.show();
            this.isShowingMore = true;
        }

        this.setModalPosition();
    }

    public getData(): ModalData {
        return new ModalData(
            this.isShowingMore,
            this.$modalTranslation.val().toString(),
            this.$modalPhraseBase.val().toString(),
            this.$modalNotes.val().toString(),
            (this.$modalState.filter(':checked').val() || '').toString(),
            this.canUndo,
            this.cancelId
        );
    }
}
