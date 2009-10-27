var $j = jQuery.noConflict();
$j(document).ready(function () {
        update_links();
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }
);

function set_refresh_timer () {
    window.setTimeout(function() {
     $('#id').empty();
    }, 3000);
};
function update_links(prnt_div) {
    if (prnt_div) {
        prnt_div.find('a.user_link').click(function () {
            prnt = $j(this).parents().filter('div.main_div');
            prnt.find('.presently_update_box').val($j(this).attr("rel"));
        });   
        prnt_div.find('a.more_text_link').click(function () {
            $j(this).parent().find('.more_text').toggle();
        });
        prnt_div.find('a[rel=lightbox]').lightBox();                     
     } else {
        $j('a.user_link').click(function () {
            prnt = $j(this).parents().filter('div.main_div');
            prnt.find('.presently_update_box').val($j(this).attr("rel"));
        });   
        $j('a.more_text_link').click(function () {
            $j(this).parent().find('.more_text').toggle();
        });
        $j('a[rel=lightbox]').lightBox();                     
     }
};
function highlight_tweets(prnt) {
        prnt.find('.twitterTimeline').effect("highlight", { }, 800);
;
};
function BeginRequestHandler(sender, args)
{
    prnt = $j(args._postBackElement).parents().filter('div.main_div');
    prnt.find('.loading_div').show();
};

function EndRequestHandler(sender, args)
{
    prnt = $j('#'+args._dataItems.refreshBoxId).parents().filter('div.main_div');
    prnt.find('.loading_div').hide();
    update_links(prnt);
    highlight_tweets(prnt);
};