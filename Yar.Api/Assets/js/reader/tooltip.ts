import * as $ from 'jquery';

import { Helper } from './helper';

export class Tooltip {
    private $tooltip: JQuery;
    private $attachedElement: JQuery;
    private readonly $readingContainer: JQuery;
    private isVisible: boolean;

    constructor(readingElement: HTMLElement) {
        this.$readingContainer = $(readingElement);
        this.$tooltip = this.$readingContainer.find('span.tooltip');
    }

    public showTooltip(attachedElement: JQuery, translation: string, modalIsOpen: boolean): void {
        this.$tooltip.html(translation);
        this.$attachedElement = attachedElement;
        this.$tooltip.removeClass('hidden');
        this.isVisible = true;

        this.setTooltipPosition(modalIsOpen);
    }

    private setTooltipPosition(modalIsOpen: boolean) {
        if (Helper.isEmpty(this.$attachedElement)) {
            return;
        }

        const offset = this.$attachedElement.offset();
        const elementHeight = this.$attachedElement.height();
        const tooltipHeight = this.$tooltip.height();
        const readingContainerOffset = this.$readingContainer.offset();

        if (modalIsOpen) {
            const newTop = offset.top - readingContainerOffset.top - tooltipHeight - elementHeight;

            if (newTop < 0) {
                this.$tooltip.css('top', offset.top - readingContainerOffset.top - tooltipHeight - elementHeight + 10);
            } else {
                this.$tooltip.css('top', offset.top - readingContainerOffset.top + elementHeight);
            }
        } else {
            const newTop = offset.top - readingContainerOffset.top - tooltipHeight - elementHeight;

            if (newTop < 0) {
                this.$tooltip.css('top', offset.top - readingContainerOffset.top + elementHeight);
            } else {
                this.$tooltip.css('top', offset.top - readingContainerOffset.top - tooltipHeight - elementHeight + 10);
            }
        }

        this.$tooltip.css('left', offset.left - readingContainerOffset.left);
    }

    public hideTooltip(): void {
        this.isVisible = false;
        this.$tooltip.addClass('hidden');
    }

    public getTooltip(): JQuery {
        return this.$tooltip;
    }
}
