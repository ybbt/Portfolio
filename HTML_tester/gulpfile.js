var gulp = require('gulp');


var less = require('gulp-less');
var path = require('path');
let cleanCSS = require('gulp-clean-css');
var concat = require('gulp-concat');
var pug = require('gulp-pug');
var htmlbeautify = require('gulp-html-beautify');
var rename = require("gulp-rename");
var notify = require("gulp-notify");
var plumber = require('gulp-plumber');
var browserSync = require('browser-sync');
var autoprefixer = require('gulp-autoprefixer');
var clean = require('gulp-clean');
const webp = require('gulp-webp');


// gulp.task('less', function () {
// 	return gulp.src(['./dev/**/*.less', '!./dev/**/libs/**/*.less'])
// 		.pipe(less({}))
// 		.pipe(concatCss("css/index.css"))
// 		.pipe(gulp.dest('./build/'));
// });




gulp.task('pug', function build() {
	// var optionsB = {
	// 	indentSize: 2,
	// 	unformatted: [
	// 		// https://www.w3.org/TR/html5/dom.html#phrasing-content
	// 		 'abbr', 'area', 'b', 'bdi', 'bdo', 'br', 'cite',
	// 		'code', 'data', 'datalist', 'del', 'dfn', 'em', 'embed', 'i', 'ins', 'kbd', 'keygen', 'map', 'mark', 'math', 'meter', 'noscript',
	// 		'object', 'output', 'progress', 'q', 'ruby', 's', 'samp', 'small',
	// 		 'strong', 'sub', 'sup', 'template', 'time', 'u', 'var', 'wbr', 'text',
	// 		'acronym', 'address', 'big', 'dt', 'ins', 'strike', 'tt'
	// 	]
	// };
	return gulp.src(['./dev/**/*.pug', '!./dev/**/__*.pug'])
		.pipe(plumber({
			errorHandler: notify.onError()
		}))
		.pipe(pug({pretty: true}))
		// .pipe(htmlbeautify(optionsB))
		.pipe(rename({dirname: ''}))
		.pipe(gulp.dest('./test_build/'))

});



gulp.task('less', function () {
	return gulp.src(['./dev/general/**/*.less', './dev/pages/**/*.less', './dev/bem/**/*.less', '!./dev/**/libs/**/*.less'])
		.pipe(plumber({
			errorHandler: notify.onError()
		}))
		.pipe(concat('index.less'))
		.pipe(less({}))
		.pipe(autoprefixer(['last 15 versions', '> 1%', 'ie 8', 'ie 7'], { cascade: true }))
		.pipe(gulp.dest('./test_build/css/'))
		// .pipe(gulp.dest('./buildt2'))

});

gulp.task('js', function () {
	return gulp.src(['./dev/pages/**/*.js', './dev/bem/**/*.js', '!./dev/**/libs/**/*.js'])
		.pipe(plumber({
			errorHandler: notify.onError()
		}))
		.pipe(concat('index.js'))		
		.pipe(gulp.dest('./test_build/js/'))

});

gulp.task('dirClean', function () {
	return gulp.src(['./test_build/img/*.jpg', './test_build/img/*.png'], {read: false})
		.pipe(clean());
});

gulp.task('dirCleanVideo', function () {
	return gulp.src(['./test_build/video/*.*'], {read: false})
		.pipe(clean());
});

gulp.task('dirCleanFonts', function () {
	return gulp.src(['./test_build/fonts/*.*'], {read: false})
		.pipe(clean());
});

gulp.task('img', function () {
	return gulp.src(['./dev/img/*.*'])
		.pipe(plumber({
			errorHandler: notify.onError()
		}))
		.pipe(gulp.dest('./test_build/img/'))

});

gulp.task('video', function () {
	return gulp.src(['./dev/video/*.*'])
		.pipe(plumber({
			errorHandler: notify.onError()
		}))
		.pipe(gulp.dest('./test_build/video/'))

});

gulp.task('fonts', function () {
	return gulp.src(['./dev/fonts/*.*'])
		.pipe(plumber({
			errorHandler: notify.onError()
		}))
		.pipe(gulp.dest('./test_build/fonts/'))

});

gulp.task('webp', () =>
	gulp.src(['./dev/img/*.jpg', './dev/img/*.png'])
		.pipe(webp())
		.pipe(gulp.dest('./test_build/img/'))
);



gulp.task('watch', function () {
	gulp.watch('./dev/**/*.less', gulp.series(['less']));
	gulp.watch('./dev/**/*.pug', gulp.series(['pug']));
	gulp.watch('./dev/**/*.js', gulp.series(['js']));
	gulp.watch('./dev/img/', gulp.series(['dirClean', 'webp', 'img']));
	gulp.watch('./dev/video/', gulp.series(['dirCleanVideo', 'video']));
	gulp.watch('./dev/fonts/', gulp.series(['dirCleanFonts', 'fonts']));
}); //остановить таск - ctrl+C


gulp.task('default', gulp.series(['watch']));


///////////////////////////\general\libs\slick-1.8.1\slick

gulp.task('pugFin', function build() {
	return gulp.src(['./dev/**/*.pug', '!./dev/**/__*.pug'])
		.pipe(plumber({
			errorHandler: notify.onError()
		}))
		.pipe(pug({pretty: true}))
		.pipe(rename({dirname: ''}))
		.pipe(gulp.dest('./build/'))

});

gulp.task('lessFin', function () {
	return gulp.src(['./dev/general/**/*.less', './dev/pages/**/*.less', './dev/bem/**/*.less', '!./dev/**/libs/**/*.less'])
		.pipe(plumber({
			errorHandler: notify.onError()
		}))
		.pipe(concat('index.less'))
		.pipe(less({}))
		.pipe(autoprefixer(['last 15 versions', '> 1%', 'ie 8', 'ie 7'], { cascade: true }))
		.pipe(gulp.dest('./build/css/'))

});

gulp.task('jsFin', function () {
	return gulp.src(['./dev/pages/**/*.js', './dev/bem/**/*.js', '!./dev/**/libs/**/*.js'])
		.pipe(plumber({
			errorHandler: notify.onError()
		}))
		.pipe(concat('index.js'))		
		.pipe(gulp.dest('./build/js/'))

});

gulp.task('normalizeFin', function () {
	return gulp.src(['./dev/general/abstract/normalize.css'])
		.pipe(plumber({
			errorHandler: notify.onError()
		}))
		.pipe(gulp.dest('./build/css/'))

});

gulp.task('icoFin', function () {
	return gulp.src(['./dev/favicon.*'])
		.pipe(plumber({
			errorHandler: notify.onError()
		}))
		.pipe(gulp.dest('./build/'))

});

gulp.task('slickFin', function () {
	return gulp.src(['./dev/general/libs/slick-1.8.1/slick/*.*'])
		.pipe(plumber({
			errorHandler: notify.onError()
		}))
		.pipe(gulp.dest('./build/libs/slick-1.8.1/slick/'))

});

gulp.task('videoFin', function () {
	return gulp.src(['./dev/video/*.*'])
		.pipe(plumber({
			errorHandler: notify.onError()
		}))
		.pipe(gulp.dest('./build/video/'))

});

gulp.task('fontsFin', function () {
	return gulp.src(['./dev/general/fonts/*.*'])
		.pipe(plumber({
			errorHandler: notify.onError()
		}))
		.pipe(gulp.dest('./build/fonts/'))

});

gulp.task('imgFin', function () {
	return gulp.src(['./dev/img/*.*'])
		.pipe(plumber({
			errorHandler: notify.onError()
		}))
		.pipe(gulp.dest('./build/img/'))

});

gulp.task('webpFin', () =>
	gulp.src(['./dev/img/*.jpg', './dev/img/*.png'])
		.pipe(webp())
		.pipe(gulp.dest('./build/img/'))
);

gulp.task('finBuild' , gulp.series(['pugFin', 'lessFin', 'jsFin', 'webpFin','imgFin', 'videoFin', 'fontsFin', 'normalizeFin', 'slickFin', 'icoFin']));

