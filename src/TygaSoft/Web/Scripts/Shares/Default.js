
var Default = {
    Init: function () {
        this.InitData();
        //this.InitClick();
    },
    InitData: function () {
        var winh = $(window).height();
        var h = winh - 107 - 100;
        $("#img_split").css({ "height": "" + h + "px" });
        var imgArr = new Array();
        $("#editorContent img").each(function () {
            imgArr.push($(this).attr("src"));
        })
        var ul = $("#img_split ul:first");
        var firstLi = ul.find("li:first");
        for (var i = 0; i < imgArr.length; i++) {img_split
            var newLi = firstLi.clone();
            newLi.find("img").attr("src", imgArr[i]);
            ul.append(newLi);
        }
        firstLi.remove();
        $("#editorContent").remove();

        $("#img_split ul>li img").css({ "width": "100%", "height": "" + h + "px" });

        $('.default-slider').unslider({
            autoplay: true,
            nav:false,
            animateHeight :true,
            arrows: {
                prev: '<a class="unslider-arrow prev"></a>',
                next: '<a class="unslider-arrow next"></a>',
            }
        });
        var toph = ($('.unslider').height() + 113) / 2;
        $(".unslider").find(".next").css("top", toph).end().find(".prev").css("top", toph);
    },
    InitClick:function(){
        $("#img_split").find(".btn_left>a").click(function () {
            //var slider = $('.default-slider').unslider();
            //slider.unslider('prev');
            //$("#img_split>ul>li:visible").prev().fadeIn().siblings().hide();

            return false;
        })
        $("#img_split").find(".btn_right>a").click(function () {
            //var slider = $('.default-slider').unslider();
            //slider.unslider('next');
            //$("#img_split>ul>li:visible").next().fadeIn().siblings().hide();

            return false;
        })

        $("#img_split ul>li img").hover(function () {
            Default.ImgSlide = 0;
        }, function () {
            Default.ImgSlide = 1;
        })

        //setInterval(Default.AutoRun,5000);
    },
    ImgSlide: 1,
    AutoRun: function () {
        if (Default.ImgSlide == 1) {
            var li = null;
            var lastIndex = $("#img_split>ul>li:last").index();
            //console.log("lastIndex--"+lastIndex);
            $("#img_split>ul>li").each(function (i, item) {
                if ($(item).css('display') != "none") {
                    //console.log("i--" + i);
                    if (i == lastIndex) li = $("#img_split>ul>li:first");
                    else li = $(item).next();
                }
            })
            li.fadeIn().siblings().hide();
        }
    }
}