<?php if (!defined("IN_HOLOCMS")) { header("Location: $path"); exit; } ?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
        "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
	<meta http-equiv="content-type" content="text/html; charset=ISO-8859-1" />
	<title><?php echo $shortname; ?> - <?php echo $pagename; ?></title>
<script type="text/javascript">
var andSoItBegins = (new Date()).getTime();
</script>

    <link rel="shortcut icon" href="<?php echo $path; ?>/web-gallery/v2/images/favicon.ico" type="image/vnd.microsoft.icon" />
    <link rel="alternate" type="application/rss+xml" title="Habbo Hotel - RSS" href="<?php echo $path; ?>/articles/rss.xml" />

<?php if($body_id == "index"){ ?>
<script src="<?php echo $path; ?>/web-gallery/static/js/libs2.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/landing.js" type="text/javascript"></script>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/frontpage_new.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/changepassword.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/forcedemaillogin.css" type="text/css" />
<?php }elseif($body_id == "merge"){ ?>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/style.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/buttons.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/boxes.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/tooltips.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/changepassword.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/embeddedregistration.css" type="text/css" />

<script src="<?php echo $path; ?>/web-gallery/static/js/simpleregistration.js" type="text/javascript"></script>
    <script type="text/javascript" src="https://api-secure.recaptcha.net/js/recaptcha_ajax.js"></script>
<?php } else { ?>
<script src="<?php echo $path; ?>/web-gallery/static/js/visual.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/libs.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/common.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/libs2.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/landing.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/embed.js" type="text/javascript"></script>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/style.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/buttons.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/boxes.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/tooltips.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/process.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/embed.css" type="text/css" />

<?php } ?>

<script type="text/javascript">
document.habboLoggedIn = false;
var habboName = null;
var habboReqPath = "";
var habboStaticFilePath = "./web-gallery";
var habboImagerUrl = "/habbo-imaging/";
var habboPartner = "";
window.name = "habboMain";

</script>



<meta name="description" content="<?php echo $sitename; ?> is a virtual world where you can meet and make friends." />
<meta name="keywords" content="<?php echo $shortname; ?>,<?php echo $sitename; ?>,virtual world,play games,enter competitions,make friends" />

<!--[if IE 8]>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/ie8.css" type="text/css" />
<![endif]-->
<!--[if lt IE 8]>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/ie.css" type="text/css" />
<![endif]-->
<!--[if lt IE 7]>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/ie6.css" type="text/css" />
<script src="./web-gallery/static/js/pngfix.js" type="text/javascript"></script>
<script type="text/javascript">
try { document.execCommand('BackgroundImageCache', false, true); } catch(e) {}
</script>

<style type="text/css">
body { behavior: url(./web-gallery/csshover.htc); }
</style>
<![endif]-->
<meta name="build" content="9.0.47 - Login Template - HoloCMS BETA v4.6 UBER" />
</head>