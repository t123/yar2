export class ModalData {
  public readonly hasMore: boolean;
  public readonly translation: string;
  public readonly phraseBase: string;
  public readonly notes: string;
  public readonly state: string;
  public readonly canUndo: boolean;
  public readonly uuid: string;

  constructor(hasMore: boolean, translation: string, phraseBase: string, notes: string, state: string, canUndo: boolean, uuid: string) {
    this.hasMore = hasMore;
    this.translation = translation;
    this.phraseBase = phraseBase;
    this.notes = notes;
    this.state = state;
    this.canUndo = canUndo;
    this.uuid = uuid;
  }
}
