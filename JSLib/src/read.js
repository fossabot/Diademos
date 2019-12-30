import Mercury from '@postlight/mercury-parser';

export async function getArticleContents(url, htmlContent){
	const { content } = await Mercury.parse(url, {contentType: 'html', html: htmlContent,});
	return content;
	/*
	const exec = require('child_process').exec

	  exec("mercury-parser " + url + " --format=html", (err, stdout, stderr) => {
		return stdout;
	  })
	  */
}