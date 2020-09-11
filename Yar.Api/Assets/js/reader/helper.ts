export const ClassPrefix = '__';

export class Helper {
  private static sentenceMarkers = new RegExp(/[\.\?!。]/);

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
}
