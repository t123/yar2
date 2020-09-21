import * as $ from 'jquery';
import { TextReadModel, ReadWordResponseModel, UndoResponseModel, RetranslateResponseModel, UndoRequestModel, TranslationRequestModel, SavePhraseRequestModel, ChangePhraseStateRequestModel } from './models';
import { MouseTracking } from './mouse-tracking';
import { Modal } from './modal';
import { Pager } from './pager';
import { SelectedPhrase } from './selected-phrase';
import { Helper, ClassPrefix } from './helper';
import { Tooltip } from './tooltip';
import { SelectedPhraseData } from './selected-phrase-data';
import { ModalData } from './modal-data';

class Api {
    private text: TextReadModel;

    public getText(textId: number, asParallel: boolean): Promise<TextReadModel> {
        return fetch(`/read/read/${textId}?asParallel=${asParallel}`)
            .then(response => response.json())
            .then(model => {
                this.text = model as TextReadModel;
                return this.text;
            });
    }

    public undo(uuid: string): Promise<UndoResponseModel> {
        const model: UndoRequestModel = {
            uuid
        };

        return fetch(`/read/undo/`,
            {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(model)
            })
            .then(response => response.json());
    }

    public translatePhrase(phrase: string, sentence: string): Promise<ReadWordResponseModel> {
        const model: TranslationRequestModel = {
            phrase,
            sentence,
            languageId: this.text.languageId,
            userId: this.text.userId,
            method: null
        };

        return fetch(`/read/translate/`,
            {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(model)
            })
            .then(response => response.json());
    }

    public retranslatePhrase(phrase: string, method: string): Promise<RetranslateResponseModel> {
        const model: TranslationRequestModel = {
            phrase,
            method,
            sentence: '',
            languageId: this.text.languageId,
            userId: this.text.userId
        };

        return fetch(`/read/retranslate/`,
            {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(model)
            })
            .then(response => response.json());
    }

    public savePhrase(selectedPhraseData: SelectedPhraseData, modalData: ModalData): Promise<ReadWordResponseModel> {
        const model: SavePhraseRequestModel = {
            userId: this.text.userId,
            languageId: this.text.languageId,
            phrase: selectedPhraseData.phrase,
            translation: modalData.translation,
            sentence: selectedPhraseData.sentence,
            hasMore: modalData.hasMore,
            state: modalData.state,
            phraseBase: modalData.phraseBase,
            notes: modalData.notes
        };

        return fetch(`/read/save`,
            {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(model)
            })
            .then(response => response.json());
    }

    public cycleState(phrase: string): Promise<ReadWordResponseModel> {
        let model: ChangePhraseStateRequestModel = {
            languageId: this.text.languageId,
            phrase: phrase
        };

        return fetch(`/read/updatestate`,
            {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(model)
            })
            .then(response => response.json());
    }
}

export class Reader {
    private readingElement: HTMLElement;
    private text: TextReadModel;
    private $readingContainer: JQuery;

    private mouseTracking: MouseTracking;
    private modal: Modal;
    private tooltip: Tooltip;
    private pager: Pager;
    private selectedPhrase: SelectedPhrase;
    private api: Api;

    constructor() {
        this.api = new Api();

        this.readingElement = document.getElementById('readerContainer');
        this.$readingContainer = $(this.readingElement).find('.reading-container');
        let textId: number = $(this.readingElement).data('text-id');
        let asParallel: boolean = $(this.readingElement).data('as-parallel');

        this.api.getText(textId, asParallel)
            .then(data => {
                this.text = data;
                this.setup();
            });
    }

    private setup() {
        this.mouseTracking = new MouseTracking();
        this.selectedPhrase = new SelectedPhrase();

        this.modal = new Modal(this.readingElement);
        this.tooltip = new Tooltip(this.readingElement);
        this.pager = new Pager(this.readingElement);

        this.$readingContainer.prepend(this.text.html);

        this.eventTermOnHover = this.eventTermOnHover.bind(this);
        this.eventTermOnHoverOff = this.eventTermOnHoverOff.bind(this);
        this.eventTermOnMouseDown = this.eventTermOnMouseDown.bind(this);
        this.eventTermOnMouseMove = this.eventTermOnMouseMove.bind(this);
        this.eventTermOnMouseUp = this.eventTermOnMouseUp.bind(this);
        this.eventModalToggleShowMore = this.eventModalToggleShowMore.bind(this);
        this.eventModalOnSave = this.eventModalOnSave.bind(this);
        this.eventChangePage = this.eventChangePage.bind(this);
        this.eventModalLinkClick = this.eventModalLinkClick.bind(this);
        this.eventTermRightClick = this.eventTermRightClick.bind(this);

        this.$readingContainer.on('mouseover', `span.${ClassPrefix}term`, this.eventTermOnHover);
        this.$readingContainer.on('mouseout', `span.${ClassPrefix}term`, this.eventTermOnHoverOff);
        this.$readingContainer.on('mousedown', `span.${ClassPrefix}term`, this.eventTermOnMouseDown);
        this.$readingContainer.on('mousemove', `span.${ClassPrefix}term`, this.eventTermOnMouseMove);
        this.$readingContainer.on('mouseup', `span.${ClassPrefix}term`, this.eventTermOnMouseUp);
        this.$readingContainer.on('contextmenu', `span.${ClassPrefix}term`, this.eventTermRightClick);
        $(this.readingElement).on('click', '.modal .show-more', this.eventModalToggleShowMore);
        $(this.readingElement).on('click', '.modal button.save', this.eventModalOnSave);
        $(this.readingElement).on('click', '.modal a', this.eventModalLinkClick);
        $(this.readingElement).on('click', '.pager button.previous', { direction: 'previous' }, this.eventChangePage);
        $(this.readingElement).on('click', '.pager button.next', { direction: 'next' }, this.eventChangePage);

        this.setupReadingContainerStyles();

        $(this.readingElement).on('click', event => {
            if (event.target.id === 'readerContainer' && this.modal.isModalVisible()) {
                this.modal.hideModal();
                return;
            }

            if (event.target.nodeName === 'TD' && event.target.className === 'disabled') {
                this.modal.hideModal();
                return;
            }
        });

        $(window).on('resize', () => {
            this.pager.calculatePages();
        });

        $(document).on('keydown', null, null, event => {
            // TODO
            if (this.modal.isModalVisible()) {
                if (event.ctrlKey && (event.keyCode === 13 || event.keyCode === 10)) {
                    this.modal.addNewline();
                    return;
                }

                if (event.keyCode === 13) {
                    event.preventDefault();
                    this.modalSave();
                    this.modal.hideModal();
                    return;
                }

                if (event.keyCode === 27) {
                    this.closeModal();
                    return;
                }

                if (event.keyCode === 40) {
                    event.preventDefault();
                    this.modal.toggleShowMore();
                    return;
                }
            }
        });

        if (!this.text.options.spaced) {
            $(this.readingElement).addClass('l1-not-spaced');
        }

        if (!this.text.options.l2Spaced) {
            $(this.readingElement).addClass('l2-not-spaced');
        }

        if (this.text.options.paged) {
            $(this.readingElement).addClass('has-paging');
            this.pager.calculatePages();

            $(document).on('wheel', null, null, event => {
                // TODO
                if (!this.modal.isModalVisible()) {
                    const deltaY = (<any>event.originalEvent).deltaY;

                    if (deltaY < 0) {
                        this.pager.previous();
                        return;
                    } else {
                        this.pager.next();
                        return;
                    }
                }
            });

            $(document).on('keydown', null, null, event => {
                // TODO
                if (!this.modal.isModalVisible()) {
                    if (event.which === 37) {
                        this.pager.previous();
                        return;
                    }

                    if (event.which === 39 || event.which === 32) {
                        this.pager.next();
                        return;
                    }
                }
            });
        } else {
            $(this.readingElement).addClass('no-paging');
        }
    }

    private setupReadingContainerStyles() {
        if (this.text.asParallel) {
            this.$readingContainer.css('width', `100%`);
        } else {
            this.$readingContainer.css('width', `${this.text.options.singleViewPercentage}%`);
        }

        if (this.text.asParallel) {
            this.$readingContainer.css('font-size', `${this.text.options.parallelFontSize}rem`);
        } else {
            this.$readingContainer.css('font-size', `${this.text.options.singleFontSize}rem`);
        }

        if (this.text.asParallel) {
            this.$readingContainer.css('line-height', `${this.text.options.parallelLineHeight}`);
        } else {
            this.$readingContainer.css('line-height', `${this.text.options.singleLineHeight}`);
        }

        const $table = this.$readingContainer.find('table.reading-table');
        const $pager = $(this.readingElement).find('.pager');

        $(this.readingElement).css('background-color', this.text.options.backgroundColor);
        $(this.$readingContainer).css('font-family', this.text.options.fontFamily);
        $table.css('color', this.text.options.fontColor);

        if (this.text.asParallel) {
            const $tdActive = this.$readingContainer.find('table.reading-table td.__active');
            $tdActive.css('border-right', `1px solid ${this.text.options.fontColor}`);
        }

        if ((this.text.options.highlightLines === 'Both' || this.text.options.highlightLines === 'Single') && !this.text.asParallel) {
            $('table.reading-table.single tr').on('mouseenter', (event) => {
                $(event.currentTarget).css('background-color', this.text.options.highlightLinesColour);
            });

            $('table.reading-table.single tr').on('mouseleave', () => {
                $('table.reading-table.single tr').css('background-color', '');
            });
        } else if ((this.text.options.highlightLines === 'Both' || this.text.options.highlightLines === 'Parallel') && this.text.asParallel) {
            $('table.reading-table.parallel tr').on('mouseenter', (event) => {
                $(event.currentTarget).css('background-color', this.text.options.highlightLinesColour);
            });

            $('table.reading-table.parallel tr').on('mouseleave', () => {
                $('table.reading-table.parallel tr').css('background-color', '');
            });
        }

        $table.css('background-color', this.text.options.backgroundColor);
        $pager.css('background-color', this.text.options.backgroundColor);
    }

    // #region events
    private eventChangePage(event: JQueryEventObject): void {
        if (!event || !event.data) {
            return;
        }

        this.modal.hideModal();

        const d: any = event.data;

        if (d.direction === 'next') {
            this.pager.next();
        } else {
            this.pager.previous();
        }
    }

    private closeModal() {
        const modalData = this.modal.getData();

        if (modalData.canUndo) {
            this.api.undo(this.modal.getData().uuid).then(response => this.onUndo(response));
        }

        this.modal.hideModal();
    }

    private eventModalLinkClick(event: JQueryEventObject): void {
        const anchor = $(event.target);

        if (anchor.hasClass('cancel')) {
            this.closeModal();
            return;
        }

        if (anchor.hasClass('forvo')) {
            window.open(`https://forvo.com/search/${this.selectedPhrase.getPhrase()}/${this.text.options.forvoLanguageCode}/`, 'forvo');
            return;
        }

        if (anchor.hasClass('custom')) {
            const dictionaryUrl = anchor.data('url');
            const customUrl = dictionaryUrl.replace('[[word]]', encodeURIComponent(this.selectedPhrase.getPhrase()));
            window.open(customUrl, 'custom');
            return;
        }

        if (anchor.hasClass('translate-google')) {
            window.open(`${this.text.options.googleTranslateUrl}${this.selectedPhrase.getSentence()}`, 'googletranslate');
            return;
        }

        if (anchor.hasClass('translate-deepl')) {
            window.open(`${this.text.options.deepLUrl}${this.selectedPhrase.getSentence()}`, 'deepl');
            return;
        }

        if (anchor.hasClass('bing')) {
            this.api.retranslatePhrase(this.selectedPhrase.getPhrase(), 'bing');
            return;
        }

        if (anchor.hasClass('google')) {
            this.api.retranslatePhrase(this.selectedPhrase.getPhrase(), 'google');
            return;
        }
    }

    public onRetranslatedResponse(response: RetranslateResponseModel) {
        this.modal.setTranslation(response.translation);
    }

    public onUndo(response: UndoResponseModel) {
        this.selectedPhrase.setUndoResponse(response);
    }

    private eventModalToggleShowMore(event: JQueryEventObject): void {
        this.modal.toggleShowMore();
    }

    private eventModalOnSave(event: JQueryEventObject): void {
        this.modalSave();
    }

    private modalSave() {
        this.api.savePhrase(this.selectedPhrase.getData(), this.modal.getData())
            .then(response => this.onPhraseSavedResponse(response));
    }

    private getEventTarget(event: JQueryEventObject): JQuery {
        const $target = jQuery(event.target as HTMLElement);
        return $target;
    }

    private eventTermRightClick(event: JQueryEventObject): boolean {
        const $target = this.getEventTarget(event);

        if (Helper.elementIsTerm($target)) {
            const native = $target.data('lower');
            this.copyToClipboard(native);
            let sentence = Helper.getSentence($target);

            if (this.text.options.hasGoogleTranslate) {
                window.open(`${this.text.options.googleTranslateUrl}${sentence}`, 'googletranslate');
            }

            if (this.text.options.hasDeepL) {
                window.open(`${this.text.options.deepLUrl}${sentence}`, 'deepl');
            }

            return false;
        }
    }

    private eventTermOnHover(event: JQueryEventObject): void {
        let $target = this.getEventTarget(event);

        if (Helper.isInFragment($target)) {
            $target = Helper.getElementFragment($target);
        }

        $target.addClass('hover');

        let display = '';

        if (this.text.options.showDefinitions) {
            display += $target.attr('data-definition');
        }

        if (this.text.options.showTermStatistics) {
            const commonness = $target.attr('data-frequency-commonness');
            const occurrences = $target.attr('data-occurrences');
            const frequencyNotseen = $target.attr('data-frequency-notseen');
            const frequencyTotal = $target.attr('data-frequency-total');

            if (Helper.isUndefined(commonness) || Helper.isUndefined(occurrences) || Helper.isUndefined(frequencyNotseen) || Helper.isUndefined(frequencyTotal)) {
                return;
            }

            if (display.length > 0) {
                display += '<br />';
            }

            display += `${occurrences} occurrences`;
            display += ` (${frequencyNotseen}% / `;
            display += `${frequencyTotal}%`;

            if (commonness) {
                display += ` / ${commonness} `;
            }

            display += ')';
        }

        if (display && display.length > 0) {
            this.tooltip.showTooltip($target, display, this.modal.isModalVisible());
        }
    }

    private eventTermOnHoverOff(event: JQueryEventObject): void {
        $(event.target).removeClass('hover');

        if (this.text.options.showDefinitions) {
            this.tooltip.hideTooltip();
        }
    }

    private eventTermOnMouseDown(event: JQueryEventObject): void {
        if (event.which === 2 || event.button === 4) {
            this.mouseTracking.middle = true;
            return;
        }

        if (event.which !== 1) {
            return;
        }

        this.mouseTracking.middle = false;
        this.mouseTracking.down = true;
        this.mouseTracking.dragging = false;
        this.mouseTracking.originalSpan = null;
    }

    private eventTermOnMouseMove(event: JQueryEventObject): void {
        if (event.which === 1 && this.mouseTracking.down) {
            this.mouseTracking.middle = false;
            this.mouseTracking.down = true;
            this.mouseTracking.dragging = true;

            if (this.mouseTracking.originalSpan === null) {
                this.mouseTracking.originalSpan = this.getEventTarget(event);;
            }

            if (Helper.elementIsTerm(this.getEventTarget(event))) {
                $(event.target).addClass(`${ClassPrefix}selecting`);
            }
        }
    }

    private eventTermOnMouseUp(event: JQueryEventObject): void {
        if (event.which === 2 || event.button === 4) {
            this.mouseTracking.middle = false;
            const $element: JQuery = this.getEventTarget(event);

            if (!Helper.elementIsTerm($element) || Helper.isFragment($element)) {
                return;
            }

            var phrase = $element.data('lower');
            this.api.cycleState(phrase)
                .then(response => {
                    this.selectedPhrase.select($element);
                    this.onPhraseSavedResponse(response);
                    this.selectedPhrase.deselect();
                }).catch(reason => {
                    console.log(reason);
                });

            return;
        }

        if (event.which !== 1) {
            return;
        }

        $(`.${ClassPrefix}term.${ClassPrefix}selecting`).removeClass(`${ClassPrefix}selecting`);

        let $element: JQuery = this.getEventTarget(event);
        if (this.mouseTracking.originalSpan !== null && !$element.is(this.mouseTracking.originalSpan)) {
            // We have a fragment
            const $fragment = this.createFragment(this.mouseTracking.originalSpan, $element);
            if (Helper.isEmpty($fragment)) {
                this.mouseTracking.reset();
                return;
            }
            if (!$element.is($fragment)) {
                $element = $fragment;
            }
        } else {
            if (Helper.any($element.closest(`.${ClassPrefix}fragment`))) {
                $element = $element.closest(`.${ClassPrefix}fragment`);
            }
        }

        this.onPhraseSelected($element);
        this.mouseTracking.reset();
    }

    // #endregion

    private onPhraseSelected($element: JQuery) {
        if (this.modal.isModalVisible() && this.selectedPhrase.isSameAs($element)) {
            this.selectedPhrase.deselect();
            this.modal.hideModal();
            return;
        }

        this.selectedPhrase.select($element);

        this.api.translatePhrase(this.selectedPhrase.getPhrase(), this.selectedPhrase.getSentence())
            .then(response => {
                this.copyToClipboard(this.selectedPhrase.getPhrase());
                this.onPhraseSelectedResponse(response);
            });
    }

    private copyToClipboard(phrase: string): void {
        if (!this.text.options.copyClipboard) {
            return;
        }

        (<any>navigator).clipboard.writeText(phrase);
    }

    public onPhraseSelectedResponse(response: ReadWordResponseModel) {
        if (this.text.options.hasCustomDictionary && this.text.options.customDictionaryAuto) {
            const customUrl = this.text.options.customDictionaryUrl[0].replace('[[word]]', encodeURIComponent(this.selectedPhrase.getPhrase()));
            window.open(customUrl, 'custom');
        }

        this.selectedPhrase.setTranslationResponse(response);
        this.modal.showModal(this.selectedPhrase.getElement(), response, this.text.options);
    }

    public onPhraseSavedResponse(response: ReadWordResponseModel) {
        this.selectedPhrase.setSavedResponse(response);
        this.modal.hideModal();
    }

    private createFragment(start: JQuery, end: JQuery): JQuery {
        if (!Helper.elementIsTerm(start) || !Helper.elementIsTerm(end)) {
            return start;
        }

        if (start.is(end)) {
            return start;
        }

        if (!start.parent().is(end.parent())) {
            return start;
        }

        const diff = Math.abs(start[0].offsetTop - end[0].offsetTop);
        if ((diff < 2 && start[0].offsetLeft > end[0].offsetLeft) || diff > 5) {
            // Selected backwards, same height, further on OR anywhere above
            const temp = start;
            start = end;
            end = temp;
        }

        const TempFragmentClass = `${ClassPrefix}temp_fragment`;

        start.addClass(TempFragmentClass);
        start.nextUntil(end).addClass(TempFragmentClass);
        end.addClass(TempFragmentClass);

        let inFragment = false;

        $(`.${TempFragmentClass}`).each((index, element) => {
            // Piece is already in a fragment
            if (Helper.any($(element).closest(`${ClassPrefix}fragment`))) {
                inFragment = true;
            }
        });

        if (inFragment) {
            $(`.${TempFragmentClass}`).removeClass(TempFragmentClass);
            return start;
        }

        $(`.${TempFragmentClass}`).wrapAll(`<span class="${ClassPrefix}fragment"/>`);
        $(`.${TempFragmentClass}`).each((index, element) => {
            const $element = $(element);

            if ($element.hasClass(`${ClassPrefix}kd`)) {
                $element.removeClass(`${ClassPrefix}kd`).addClass(`${ClassPrefix}kd_t`);
            }

            if ($element.hasClass(`${ClassPrefix}ud`)) {
                $element.removeClass(`${ClassPrefix}ud`).addClass(`${ClassPrefix}ud_t`);
            }

            if ($element.hasClass(`${ClassPrefix}id`)) {
                $element.removeClass(`${ClassPrefix}id`).addClass(`${ClassPrefix}id_t`);
            }

            if ($element.hasClass(`${ClassPrefix}known`)) {
                $element.removeClass(`${ClassPrefix}known`).addClass(`${ClassPrefix}known_t`);
            }

            if ($element.hasClass(`${ClassPrefix}notknown`)) {
                $element.removeClass(`${ClassPrefix}notknown`).addClass(`${ClassPrefix}notknown_t`);
            }

            if ($element.hasClass(`${ClassPrefix}ignored`)) {
                $element.removeClass(`${ClassPrefix}ignored`).addClass(`${ClassPrefix}ignored_t`);
            }

            if ($element.hasClass(`${ClassPrefix}notseen`)) {
                $element.removeClass(`${ClassPrefix}notseen`).addClass(`${ClassPrefix}notseen_t`);
            }

            if ($element.hasClass(`${ClassPrefix}current`)) {
                $element.removeClass(`${ClassPrefix}current`);
            }
        });

        $(`.${TempFragmentClass}`).removeClass(TempFragmentClass);

        return start.parent();
    }
}