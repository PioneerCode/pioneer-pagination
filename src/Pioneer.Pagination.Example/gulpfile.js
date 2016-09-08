var gulp = require("gulp");
var sass = require("gulp-sass");

function styles() {
    return gulp.src("./sass/*.scss")
      .pipe(sass({ outputStyle: "compressed" }).on("error", sass.logError))
        .pipe(gulp.dest("wwwroot/"));
}

function watch() {
    gulp.watch("./sass/**/*.scss", styles);
}

exports.styles = styles;
var build = gulp.series(styles, gulp.parallel(watch));

gulp.task("default", build);