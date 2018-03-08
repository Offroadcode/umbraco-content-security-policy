
module.exports = function(grunt) {
  require('load-grunt-tasks')(grunt);
  var path = require('path');

  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json'),
    pkgMeta: grunt.file.readJSON('config/meta.json'),
    dest: grunt.option('target') || 'dist',
    basePath: path.join('<%= dest %>', 'App_Plugins', '<%= pkgMeta.name %>'),

    watch: {
      options: {
        spawn: false,
        atBegin: true
      },
      cs: {
        files: ['ContentSecurityPolicy/**/*.cs'] ,
        tasks: ['msbuild:dist', 'copy:dll']
      },
      dll: {
        files: ['ContentSecurityPolicy/**/ContentSecurityPolicy.dll'],
        tasks: ['copy:dll']
      },
      config: {
        files: ['config/ContentSecurityPolicies.config'],
        tasks: ['copy:config']
      },
      // js: {
      //   files: ['assets/**/*.js'],
      //   tasks: ['concat:dist']
      // },
      // html: {
      //   files: ['assets/**/*.html'],
      //   tasks: ['copy:html']
      // },
      // sass: {
      //   files: ['assets/**/*.scss'],
      //   tasks: ['sass', 'copy:css']
      // },
      // css: {
      //   files: ['assets/**/*.css'],
      //   tasks: ['copy:css']
      // },
      // manifest: {
      //   files: ['assets/package.manifest'],
      //   tasks: ['copy:manifest']
      // }
    },

    concat: {
      options: {
        stripBanners: false
      },
      dist: {
        src: [
            //'assets/js/example.js',
        ],
        dest: '<%= basePath %>/js/contentSecurityPolicy.js'
      }
    },

    copy: {
        dll: {
            cwd: 'ContentSecurityPolicy/bin/debug/',
            src: 'ContentSecurityPolicy.dll',
            dest: '<%= dest %>/bin/',
            expand: true
        },
        html: {
            cwd: 'assets/views/',
            src: [
                //'ExampleView.html',
            ],
            dest: '<%= basePath %>/views/',
            expand: true,
            rename: function(dest, src) {
                return dest + src;
              }
        },
        config: {
          cwd: 'config/',
          src: [
              'ContentSecurityPolicies.config',
          ],
          dest: '<%= dest %>/Config/',
          expand: true,
          rename: function(dest, src) {
              return dest + src;
            }
      },
		css: {
			cwd: 'assets/css/',
			src: [
				//'styles.css'
			],
			dest: '<%= basePath %>/css/',
			expand: true,
			rename: function(dest, src) {
				return dest + src;
			}
		},
        manifest: {
            cwd: 'assets/',
            src: [
                //'package.manifest'
            ],
            dest: '<%= basePath %>/',
            expand: true,
            rename: function(dest, src) {
                return dest + src;
            }
        },
       umbraco: {
        cwd: '<%= dest %>',
        src: '**/*',
        dest: 'tmp/umbraco',
        expand: true
      }
    },

    umbracoPackage: {
      options: {
        name: "<%= pkgMeta.name %>",
        version: '<%= pkgMeta.version %>',
        url: '<%= pkgMeta.url %>',
        license: '<%= pkgMeta.license %>',
        licenseUrl: '<%= pkgMeta.licenseUrl %>',
        author: '<%= pkgMeta.author %>',
        authorUrl: '<%= pkgMeta.authorUrl %>',
        manifest: 'config/package.xml',
        readme: 'config/readme.txt',
        sourceDir: 'tmp/umbraco',
        outputDir: 'pkg',
      }
    },

    jshint: {
      options: {
        jshintrc: '.jshintrc'
      },
      src: {
        src: ['app/**/*.js', 'lib/**/*.js']
      }
  },

  sass: {
		dist: {
			options: {
				style: 'compressed'
			},
			files: {
				'assets/css/styles.css': 'assets/scss/styles.scss'
			}
		}
	},

  clean: {
      build: '<%= grunt.config("basePath").substring(0, 4) == "dist" ? "dist/**/*" : "null" %>',
      tmp: ['tmp'],
      html: [
        'assets/views/*.html',
        ],
      js: [
        'assets/**/*.js',
      ],
      css: [
        'assets/**/*.css',
      ],
      sass: [
        'assets/**/*.scss',
      ]
  },
  msbuild: {
      options: {
        stdout: true,
        verbosity: 'quiet',
        maxCpuCount: 4,
        version: 4.0,
        buildParameters: {
          WarningLevel: 2,
          NoWarn: 1607
        }
    },
    dist: {
        src: ['ContentSecurityPolicy/ContentSecurityPolicy.csproj'],
        options: {
            projectConfiguration: 'Debug',
            targets: ['Clean', 'Rebuild'],
        }
    }
  }

  });

  grunt.registerTask('default', [
    //'concat', 
    // 'sass:dist', 
    // 'copy:html', 
    //'copy:manifest', 
    // 'copy:css', 
    'msbuild:dist', 
    'copy:dll', 
    // 'clean:html', 
    // 'clean:js', 
    // 'clean:sass', 
    // 'clean:css'
  ]);
  grunt.registerTask('umbraco', [
    'clean:tmp', 
    'default', 
    'copy:umbraco', 
    'umbracoPackage', 
    'clean:tmp'
  ]);
};
