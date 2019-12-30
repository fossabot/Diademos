import {getArticleContents} from './read';

export async function GetArticleContents(url, htmlContent){
	return getArticleContents(url, htmlContent);
}
export async function PrintArticleContents(url, htmlContent){
	console.log(getArticleContents(url, htmlContent));
}