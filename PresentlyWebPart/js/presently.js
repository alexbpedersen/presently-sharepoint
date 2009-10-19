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
function update_links(parent_div) {
    if (parent_div) {
        parent_div.find('a.user_link').click(function () {
            parent = $j(this).parents().filter('div.main_div');
            parent.find('.presently_update_box').val($j(this).attr("rel"));
        });   
        parent_div.find('a.more_text_link').click(function () {
            $j(this).parent().find('.more_text').toggle();
        });
        parent_div.find('a[rel=lightbox]').lightBox();                     
     } else {
        $j('a.user_link').click(function () {
            parent = $j(this).parents().filter('div.main_div');
            parent.find('.presently_update_box').val($j(this).attr("rel"));
        });   
        $j('a.more_text_link').click(function () {
            $j(this).parent().find('.more_text').toggle();
        });
        $j('a[rel=lightbox]').lightBox();                     
     }
};
function highlight_tweets(parent) {
        parent.find('.twitterTimeline').effect("highlight", { }, 800);
;
};
function BeginRequestHandler(sender, args)
{
    parent = $j(args._postBackElement).parents().filter('div.main_div');
    parent.find('.loading_div').show();
};

function EndRequestHandler(sender, args)
{
    parent = $j('#'+args._dataItems.refreshBoxId).parents().filter('div.main_div');
    parent.find('.loading_div').hide();
    update_links(parent);
    highlight_tweets(parent);
};