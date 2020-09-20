export interface LanguageIndexModel
{
	id: number;
	name: string;
}
export interface LanguageViewModel
{
	id: number;
	name: string;
	regEx: string;
	translationMethod: string;
	bingTranslationUrl: string;
	bingTranslationKey: string;
	googleTranslationSource: string;
	googleTranslationTarget: string;
	googleTranslationKey: string;
	googleTranslateUrl: string;
	forvoLanguageCode: string;
	leftToRight: boolean;
	paged: boolean;
	spaced: boolean;
	multipleSentences: boolean;
	popupModal: string;
	copyClipboard: boolean;
	showDefinitions: boolean;
	showTermStatistics: boolean;
	maxFragmentParseLength: number;
	mostCommonTerms: number;
	customDictionaryUrl: string;
	customDictionaryAuto: boolean;
	centreModal: boolean;
	enableLeitner: boolean;
	leitnerMultiplier: number;
	stateOnOpen: string;
	singleViewPercentage: number;
	singleLineHeight: number;
	singleFontSize: number;
	parallelLineHeight: number;
	parallelFontSize: number;
	fontColor: string;
	backgroundColor: string;
	fontFamily: string;
}
export interface LoginModel
{
	username: string;
	password: string;
	rememberMe: boolean;
}
export interface ReadWordResponseModel
{
	phrase: string;
	phraseBase: string;
	phraseLower: string;
	translation: string;
	state: string;
	notes: string;
	canUndo: boolean;
	uuid: string;
}
export interface SavePhraseRequestModel
{
	phrase: string;
	sentence: string;
	translation: string;
	userId: number;
	languageId: number;
	state: string;
	phraseBase: string;
	notes: string;
	hasMore: boolean;
}
export interface SentenceIndexModel
{
	id: number;
	sentence: string;
	created: string;
}
export interface SignUpModel
{
	username: string;
	password: string;
}
export interface TextIndexModel
{
	id: number;
	languageName: string;
	title: string;
	collection: string;
	collectionNo?: number;
	isParallel: boolean;
	created: string;
	lastRead?: string;
}
export interface TextReadModel
{
	id: number;
	userId: number;
	languageId: number;
	languageName: string;
	title: string;
	collection: string;
	collectionNo?: number;
	isParallel: boolean;
	asParallel: boolean;
	html: string;
	options: LanguageOptionsModel;
}
export interface LanguageOptionsModel
{
	leftToRight: boolean;
	paged: boolean;
	spaced: boolean;
	multipleSentences: boolean;
	popupModal: string;
	copyClipboard: boolean;
	showDefinitions: boolean;
	showTermStatistics: boolean;
	hasGoogle: boolean;
	hasBing: boolean;
	hasGoogleTranslate: boolean;
	hasForvo: boolean;
	hasCustomDictionary: boolean;
	googleTranslateUrl: string;
	forvoLanguageCode: string;
	mostCommonTerms: number;
	customDictionaryUrl: string[];
	customDictionaryAuto: boolean;
	centreModal: boolean;
	l2LeftToRight: boolean;
	l2Spaced: boolean;
	enableLeitner: boolean;
	leitnerMultiplier: number;
	stateOnOpen: string;
	singleViewPercentage: number;
	singleLineHeight: number;
	singleFontSize: number;
	parallelLineHeight: number;
	parallelFontSize: number;
	fontColor: string;
	backgroundColor: string;
	fontFamily: string;
	hasDeepL: boolean;
	deepLUrl: string;
	highlightLines: string;
	highlightLinesColour: string;
}
export interface TextViewModel
{
	id: number;
	l1Text: string;
	l2Text: string;
	userId: number;
	languageId: number;
	language2Id?: number;
	languageName: string;
	title: string;
	collection: string;
	collectionNo?: number;
	isParallel: boolean;
	created: string;
	updated: string;
	lastRead?: string;
}
export interface TranslationRequestModel
{
	phrase: string;
	sentence: string;
	userId: number;
	languageId: number;
	method: string;
}
export interface RetranslateResponseModel
{
	phrase: string;
	phraseLower: string;
	translation: string;
}
export interface UndoRequestModel
{
	uuid: string;
}
export interface UndoResponseModel
{
	phrase: string;
	phraseLower: string;
	success: boolean;
}
export interface WordIndexModel
{
	id: number;
	languageName: string;
	phrase: string;
	translation: string;
	sentenceCount: number;
	sentences: SentenceIndexModel[];
}
