(function ($){
    $(function(){
        var latestKnownScrollY = window.scrollY;
        var $leftcontainer = $('.nav-collapse');
        var $toggler = $('.toggler');
        var windowheight;
        var panelheight,panelwidth;
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
            'position':'fixed',
            'top':0,
            'left':0,
            'bottom':0,
            'right':0,
            'height':'100%',
            'width':'100%',
            'background':'black',
            'display':'none',
            'z-index':101,
            'opacity':0
        });

        $dimmer.appendTo($('body'));
        

        function toggleNav(){
            if (isActive) {
                // Hide panel
                isActive=false;
                movePanel(0);
                startLeft = 0;
                setPanelTop();
                $toggler.removeClass('active');
            } else {
                // Show panel
                isActive=true;
                movePanel(panelwidth);
                startLeft = panelwidth;
                $toggler.addClass('active');
            }
        }

        function onScroll() {
            if (shouldIMove) {
                latestKnownScrollY = window.scrollY;
                if (!waiting) {
                    setTimeout(setPanelTop,delay);
                    waiting=true;
                }
            }

        }
        
        function setPanelTop() {
                if (isActive) {
                    var containertop = $leftcontainer.offset().top;
                    if (latestKnownScrollY<containertop) {
                        panelTop = latestKnownScrollY;
                    }
                    else if (latestKnownScrollY+windowheight > containertop+panelheight){
                        panelTop = latestKnownScrollY-(panelheight-windowheight);
                    }

                } else {
                    panelTop = latestKnownScrollY;
                }
                movePanelVertical(panelTop);
                waiting=false;
        }

        function resetPanel() {
            $leftcontainer.removeAttr('style'); // Reset the offset in the viewport for the panel;
        }


        var isTouchDevice = function (){
            try{
                document.createEvent("TouchEvent");
                return true;
            }catch(e){
                return false;
            }
        };

        function addPanelTouchGestures() {
            var moving = false, shifted=false;
            var movedistance = 0;
            var startPos = 0;
            var movethreshold = 65;
            var newleft;

            document.getElementById('toggler').addEventListener("touchstart", function(event) {
                moving = true;
                $leftcontainer.addClass('moving');
                startPos = event.touches[0].pageX;
                $dimmer.show();
                event.preventDefault();
                event.stopPropagation();
            },false);

            document.getElementById('toggler').addEventListener("touchend", function(event) {
                if (moving) {
                    moving = false;
                    $leftcontainer.removeClass('moving');
                    if (Math.abs(movedistance)>=movethreshold) {
                        toggleNav();
                    } else {
                        // reset to original position
                        movePanel(startLeft);
                    }
                    
                    event.preventDefault();
                    event.stopPropagation();
                }
            },false);

            document.getElementById('toggler').addEventListener("touchmove", function(event) {
                if (moving) {
                    movedistance = event.touches[0].pageX-startPos;
                    newleft = startLeft+movedistance;
                    if (newleft>=0 ) {
                        movePanel(newleft);
                    }
                    event.preventDefault();
                    event.stopPropagation();
                }
            },false);
        
        }

        addPanelTouchGestures();

        function movePanel(left) {
            panelLeft = left;
            if (Modernizr.csstransforms3d) {
               $leftcontainer.css('-webkit-transform', 'translate3d('+left+'px, '+panelTop+'px, 0)');
            } else {
                if (Modernizr.csstransforms) {
                    $leftcontainer.css('-webkit-transform', 'translate('+left+'px, '+panelTop+'px)');
                } else {
                    $leftcontainer.css('margin-left', left);
                }
            }

            if (left>0) {
                var opacity = (left/panelwidth)*0.5;
                $dimmer.show();
                $dimmer.css('opacity', opacity);
            } else {
                $dimmer.css('opacity', 0);
                $dimmer.hide();
            }
        }

        function movePanelVertical(top) {
            if (Modernizr.csstransforms3d) {
               $leftcontainer.css('-webkit-transform', 'translate3d('+panelLeft+'px,'+top+'px, 0)');
            } else {
                if (Modernizr.csstransforms) {
                    $leftcontainer.css('-webkit-transform', 'translate('+panelLeft+'px,'+top+'px)');
                } else {
                    $leftcontainer.css('top', top);
                }
            }
        }


        function initNavigationValues(){
            windowheight = $(window).height();
            panelheight = $('.nav-collapse').outerHeight();
            outerLeft = $leftcontainer.offset().left;
            panelwidth = $('.nav-collapse .nav').outerWidth();
            setPanelTop();
            $toggler.height(panelheight);
            $('#togglegraphics').css('top', (windowheight*0.5));

            // Activate the GPU for devices that uses css3transforms
            if (Modernizr.csstransforms3d) {
                $leftcontainer.css('-webkit-transform', 'translate3d(0, 0, 0)');
            }
        }
        /* Lets attach our functions */

        $toggler.click( toggleNav );
        $dimmer.click( toggleNav );
        $(window).bind('scroll', onScroll);

        
        if (document.documentElement.clientWidth<980) {
            initNavigationValues();
            shouldIMove = true;
        }


        // Set set variables
        $(window).resize(function() {
            if(document.documentElement.clientWidth<980) {
                initNavigationValues();
                shouldIMove = true;
            } else {
                resetPanel();
                shouldIMove = false;
            }
        });
        
    });
}(jQuery));