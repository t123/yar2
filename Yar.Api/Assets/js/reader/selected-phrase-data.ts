export class SelectedPhraseData {
  public readonly phrase: string;
  public readonly translation: string;
  public readonly sentence: string;

  constructor(phrase: string, translation: string, sentence: string) {
    this.phrase = phrase;
    this.translation = translation;
    this.sentence = sentence;
  }
}
