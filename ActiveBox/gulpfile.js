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
var autoprefixer = require('gulp-autoprefixer');
var clean = require('gulp-clean');
const webp = require('gulp-webp');
const babel = require('gulp-babel');


 
gulp.task('babel', () =>
    gulp.src(['node_modules/babel-polyfill/dist/polyfill.js', './dev/**/pages/**/*.js', './dev/**/bem/**/*.js'])
        .pipe(concat('index.js'))
        .pipe(babel({
            presets: ['@babel/env']
        }))
        .pipe(gulp.dest('./test_build/js/'))
);


gulp.task('pug', function build() {

    return gulp.src(['./dev/**/*.pug', '!./dev/**/__*.pug'])
        .pipe(plumber({
            errorHandler: notify.onError()
        }))
        .pipe(pug({pretty: true}))
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

// gulp.task('js', function () {
//     return gulp.src(['./dev/pages/**/*.js', './dev/bem/**/*.js', '!./dev/**/libs/**/*.js'])
//         .pipe(plumber({
//             errorHandler: notify.onError()
//         }))
//         .pipe(concat('index.js'))
//         .pipe(gulp.dest('./test_build/js/'))

// });

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

gulp.task('fonts', function () {
    return gulp.src(['./dev/fonts/*.*'])
        .pipe(plumber({
            errorHandler: notify.onError()
        }))
        .pipe(gulp.dest('./test_build/fonts/'))

});



gulp.task('watch', function () {
    gulp.watch('./dev/**/*.less', gulp.series(['less']));
    gulp.watch('./dev/**/*.pug', gulp.series(['pug']));
    gulp.watch('./dev/**/*.js', gulp.series(['babel']));
    // gulp.watch('./dev/**/*.js', gulp.series(['js']));
    gulp.watch('./dev/img/', gulp.series(['dirClean', /* 'webp',  */'img']));
    gulp.watch('./dev/fonts/', gulp.series(['dirCleanFonts', 'fonts']));
}); //остановить таск - ctrl+C


gulp.task('default', gulp.series(['watch']));


///////////////////////////

gulp.task('babelFin', () =>
    gulp.src(['node_modules/babel-polyfill/dist/polyfill.js', './dev/**/pages/**/*.js', './dev/**/bem/**/*.js'])
        .pipe(concat('index.js'))
        .pipe(babel({
            presets: ['@babel/env']
        }))
        .pipe(gulp.dest('./build/js/'))
);

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

// gulp.task('jsFin', function () {
//     return gulp.src(['./dev/pages/**/*.js', './dev/bem/**/*.js', '!./dev/**/libs/**/*.js'])
//         .pipe(plumber({
//             errorHandler: notify.onError()
//         }))
//         .pipe(concat('index.js'))
//         .pipe(gulp.dest('./build/js/'))

// });

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



gulp.task('finbuild' , gulp.parallel(['babelFin', 'pugFin', 'lessFin', 'imgFin', 'fontsFin', 'normalizeFin', 'slickFin', 'icoFin']));

