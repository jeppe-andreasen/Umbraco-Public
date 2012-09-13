var application = window.application || {};

/* Top Nav */

application.navigation = {
    init: function () {

        var $toggler = $('.toggler');
        if ($toggler.length == 0)
            return;

        var latestKnownScrollY = window.scrollY;
        var $leftcontainer = $('.nav-collapse');
        var $toggler = $('.toggler');
        var windowheight;
        var panelheight, panelwidth;
        var outerLeft;
        var isActive = false;
        var waiting = false;
        var delay = 800;
        var phoneWidth = '768px';
        var startLeft = 0;
        var panelTop = 0, panelLeft = 0;
        var shouldIMove = false;

        var $dimmer = $("<div></div>");
        $dimmer.css({
            'position': 'fixed',
            'top': 0,
            'left': 0,
            'bottom': 0,
            'right': 0,
            'height': '100%',
            'width': '100%',
            'background': 'black',
            'display': 'none',
            'z-index': 101,
            'opacity': 0
        });

        $dimmer.appendTo($('body'));

        var isTouchDevice = function () {
            try {
                document.createEvent("TouchEvent");
                return true;
            } catch (e) {
                return false;
            }
        };

        this.addPanelTouchGestures();

        $toggler.click(toggleNav);
        $dimmer.click(toggleNav);
        $(window).bind('scroll', onScroll);


        if (document.documentElement.clientWidth < 980) {
            initNavigationValues();
            shouldIMove = true;
        }


        // Set set variables
        $(window).resize(function () {
            if (document.documentElement.clientWidth < 980) {
                initNavigationValues();
                shouldIMove = true;
            } else {
                resetPanel();
                shouldIMove = false;
            }
        });

    },
    toggleNav: function () {
        if (isActive) {
            // Hide panel
            isActive = false;
            movePanel(0);
            startLeft = 0;
            setPanelTop();
            $toggler.removeClass('active');
        } else {
            // Show panel
            isActive = true;
            movePanel(panelwidth);
            startLeft = panelwidth;
            $toggler.addClass('active');
        }
    },
    onScroll: function () {
        if (shouldIMove) {
            latestKnownScrollY = window.scrollY;
            if (!waiting) {
                setTimeout(setPanelTop, delay);
                waiting = true;
            }
        }
    },
    setPanelTop: function () {
        if (isActive) {
            var containertop = $leftcontainer.offset().top;
            if (latestKnownScrollY < containertop) {
                panelTop = latestKnownScrollY;
            }
            else if (latestKnownScrollY + windowheight > containertop + panelheight) {
                panelTop = latestKnownScrollY - (panelheight - windowheight);
            }

        } else {
            panelTop = latestKnownScrollY;
        }
        movePanelVertical(panelTop);
        waiting = false;
    },
    resetPanel: function () {
        $leftcontainer.removeAttr('style'); // Reset the offset in the viewport for the panel;
    },
    addPanelTouchGestures: function () {
        var moving = false, shifted = false;
        var movedistance = 0;
        var startPos = 0;
        var movethreshold = 65;
        var newleft;

        if ($("#toggler").length > 0) {
            $toggler[0].addEventListener("touchstart", function (event) {
                moving = true;
                $leftcontainer.addClass('moving');
                startPos = event.touches[0].pageX;
                $dimmer.show();
                event.preventDefault();
                event.stopPropagation();
            }, false);

            $toggler[0].addEventListener("touchend", function (event) {
                if (moving) {
                    moving = false;
                    $leftcontainer.removeClass('moving');
                    if (Math.abs(movedistance) >= movethreshold) {
                        toggleNav();
                    } else {
                        // reset to original position
                        movePanel(startLeft);
                    }

                    event.preventDefault();
                    event.stopPropagation();
                }
            }, false);

            $toggler[0].addEventListener("touchmove", function (event) {
                if (moving) {
                    movedistance = event.touches[0].pageX - startPos;
                    newleft = startLeft + movedistance;
                    if (newleft >= 0) {
                        movePanel(newleft);
                    }
                    event.preventDefault();
                    event.stopPropagation();
                }
            }, false);
        }
    },
    movePanel: function (left) {
        panelLeft = left;
        if (Modernizr.csstransforms3d) {
            $leftcontainer.css('-webkit-transform', 'translate3d(' + left + 'px, ' + panelTop + 'px, 0)');
        } else {
            if (Modernizr.csstransforms) {
                $leftcontainer.css('-webkit-transform', 'translate(' + left + 'px, ' + panelTop + 'px)');
            } else {
                $leftcontainer.css('margin-left', left);
            }
        }

        if (left > 0) {
            var opacity = (left / panelwidth) * 0.5;
            $dimmer.show();
            $dimmer.css('opacity', opacity);
        } else {
            $dimmer.css('opacity', 0);
            $dimmer.hide();
        }
    },

    movePanelVertical: function (top) {
        if (Modernizr.csstransforms3d) {
            $leftcontainer.css('-webkit-transform', 'translate3d(' + panelLeft + 'px,' + top + 'px, 0)');
        } else {
            if (Modernizr.csstransforms) {
                $leftcontainer.css('-webkit-transform', 'translate(' + panelLeft + 'px,' + top + 'px)');
            } else {
                $leftcontainer.css('top', top);
            }
        }
    },
    initNavigationValues: function () {
        windowheight = $(window).height();
        panelheight = $('.nav-collapse').outerHeight();
        outerLeft = $leftcontainer.offset().left;
        panelwidth = $('.nav-collapse .nav').outerWidth();
        setPanelTop();
        $toggler.height(panelheight);
        $('#togglegraphics').css('top', (windowheight * 0.5));

        // Activate the GPU for devices that uses css3transforms
        if (Modernizr.csstransforms3d) {
            $leftcontainer.css('-webkit-transform', 'translate3d(0, 0, 0)');
        }
    }
}



/* Background Image */

application.background = {
    init: function () {
        var form = $("form");
        if (form.data("bgimage") != "") {
            form.attr("style", "background:url('" + form.data("bgimage") + "') no-repeat top center");
        }
    }
}


/* Image Carousel */

application.carousel = {
    init: function (options) {
        $('.carousel').carousel(options);
    }
}

/* Search Box */

application.search = {
    init: function (url) {
        $('.search-query').bind('keypress', function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                window.location.href = url + "?query=" + $(this).val();
            }
        });
    }
}

//$(".search-query").bind('keypress', function (e) {
//    if (e.keyCode == 13) {
//        return e.keyCode = 9; //set event key to tab
//    }
//});

/* Accordion */

application.accordion = {
    init: function () {
        
    }
};

application.bigtext = {
    init:function (element){
        $(element).bigtext();
    }
};

/* Button */

application.button = {
    init: function () {
        $('.nav-tabs').button();
    }
}

/***** Disqus Module *****/

var disqus_developer = 0;
var disqus_shortname = '';

application.disqus = {
    init: function (developerMode, shortName) {
        
        if (developerMode)
            disqus_developer = 1;
        
        disqus_shortname = shortName; 
        
        var dsq = document.createElement('script'); dsq.type = 'text/javascript'; dsq.async = true;
        dsq.src = 'http://' + disqus_shortname + '.disqus.com/embed.js';
        (document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(dsq);
    }
}

/***** Search Filter Part *****/

application.searchfilter = {
    init: function () {
        $('.search-filter .datepicker').datepicker();
    }
}


/***** Video Module *****/

application.videomodule = {
    init: function () {
        $(window).resize(function () {
            application.videomodule.correctAspectRatio();
        });
        this.correctAspectRatio();
    },
    correctAspectRatio: function () {
        $('.video-module iframe').each(function () {
            var $iframe = $(this);
            $iframe.height($iframe.width() * 0.5625);
        });
    }
}

/***** Google Analytics ******/

var _gaq = _gaq || [];
application.googleanalytics = {
    init: function (apikey) {
        
        _gaq.push(['_setAccount', apikey]);
        _gaq.push(['_trackPageview']);

        (function () {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        })();
    }
}


/***** News List *****/
application.newslist = {
    init: function () {
        $("body").on('click', '.news-list .pagination a', function (e) {
            e.preventDefault();

            var $a = $(this);
            if ($a.hasClass("disabled"))
                return;

            var page = $a.attr("data-page");
            if (page == undefined)
                return;

            var $list = $a.closest('.news-list');
            var filter = $list.attr("data-filter");
            var itemsPerPage = $list.attr("data-ipp");
            var ajaxResult = getNewsListPage(parseInt(page), filter, parseInt(itemsPerPage));
            $list.find('ul.results').html(ajaxResult.results);
            $list.find('.pagination').html(ajaxResult.pager);
        });
    }
}
