export class MouseTracking {
    private _down: boolean;
    private _dragging: boolean;
    private _$originalSpan: JQuery | null;

    constructor(down: boolean = false, dragging: boolean = false, $originalSpan: JQuery | null = null) {
        this.down = down;
        this.dragging = dragging;
        this.originalSpan = $originalSpan;
    }

    get down(): boolean {
        return this._down;
    }

    set down(value: boolean) {
        this._down = value;
    }

    get dragging(): boolean {
        return this._dragging;
    }
    set dragging(value: boolean) {
        this._dragging = value;
    }

    get originalSpan(): JQuery | null {
        return this._$originalSpan;
    }

    set originalSpan(value: JQuery | null) {
        this._$originalSpan = value;
    }

    reset(): void {
        this.down = false;
        this.dragging = false;
        this.originalSpan = null;
    }
}
