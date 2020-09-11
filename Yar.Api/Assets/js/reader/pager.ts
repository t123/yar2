import * as $ from 'jquery';

export class Pager {
    private $pager: JQuery<HTMLElement>;
    private readonly $readingContainer: JQuery<HTMLElement>;
    private readonly $pageCounter: JQuery<HTMLElement>;
    private readonly PageMargin = 30;
    private _currentPage = 1;

    constructor(readingElement: HTMLElement) {
        this.$readingContainer = $(readingElement).find('.reading-container');
        this.$pager = $(readingElement).find('div.pager');
        this.$pageCounter = $(readingElement).find('.page-counter');
    }

    public getPager(): JQuery<HTMLElement> {
        return this.$pager;
    }

    public previous(): void {
        const $readingTable = this.$readingContainer.find('table.reading-table');
        const offset = $readingTable.offset();

        if (offset.top < 70) {
            offset.top = offset.top + (this.$readingContainer.height() - this.PageMargin);

            if (offset.top > 70) {
                offset.top = 70;
            } else {
                this._currentPage--;
            }

            $readingTable.offset(offset);
        }

        this.calculatePages();
    }

    public next(): void {
        const $readingTable = this.$readingContainer.find('table.reading-table');

        if ($readingTable.height() > this.$readingContainer.height()) {
            const offset = $readingTable.offset();
            offset.top = offset.top - (this.$readingContainer.height() - this.PageMargin);

            if (offset.top * -1 > $readingTable.height()) {
                return;
            }

            $readingTable.offset(offset);
            this._currentPage++;
        }

        this.calculatePages();
    }

    public calculatePages(): void {
        const $readingTable = this.$readingContainer.find('table.reading-table');
        const pageCount = Math.ceil($readingTable.height() / (this.$readingContainer.height() - this.PageMargin));

        this.$pageCounter.html(`${this._currentPage} / ${pageCount}`);
    }
}
