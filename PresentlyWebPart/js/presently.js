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
function update_links() {
        $j('a.user_link').click(function () {
            $j('.presently_update_box').val($j(this).attr("rel"));
        });   
        $j('a.more_text_link').click(function () {
            $j(this).parent().find('.more_text').toggle();
        });
        $j('a[rel=lightbox]').lightBox();                     
};
function highlight_tweets() {
        $j('.twitterTimeline').effect("highlight", { }, 800);
;
};
function BeginRequestHandler(sender, args)
{
    $j('.loading_div').show();
};

function EndRequestHandler(sender, args)
{
 $j('.loading_div').hide();
 update_links();
 highlight_tweets();
};