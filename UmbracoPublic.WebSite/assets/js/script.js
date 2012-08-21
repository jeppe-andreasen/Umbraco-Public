var application = window.application || {};


/* Background Image */

application.background = {
    init: function () {
        var form = $("form");
        if (form.data("bgimage") != "") {
            form.attr("style", "background:url('" + form.data("bgimage") + "') no-repeat top center");
        }
    }
}


/* Search Box */

application.search = {
    init: function () {
        $('.search-query').bind('keypress', function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                window.location.href = "/test/SearchResult.aspx?query=" + $(this).val();
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

/* Button */

application.button = {
    init: function () {
        $('.nav-tabs').button();

        /*
        $('.alert').each(function () {

            var span = $(this);
            var div = $('<div class="alert"><button class="close" data-dismiss="alert">×</button><span></span></div>');
            div.attr("class", span.attr("class"));
            div.find("span").html(span.html());
            span.replaceWith(div);
        });

        */


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