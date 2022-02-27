<?php if (!defined("IN_HOLOCMS")) { header("Location: $path"); exit; }

if(!isset($pagename) || empty($pagename)){
$pagename = $shortname;
}

?>
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="de" lang="de">
<head>
<base href="<?php echo $path; ?>">
<?php if($pageid == "profile" || $pageid == "10b" || $pageid == "13" || $pageid == "com"){ ?>
	<meta http-equiv="content-type" content="text/html; charset=utf-8" />
	<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
	<?php header("Content-Type: text/html; charset=iso-8859-1",true); ?>
<?php } else { ?>
	<meta http-equiv="content-type" content="text/html; charset=ISO-8859-1" />
	<?php header("Content-Type: text/html; charset=iso-8859-1",true); ?>
<?php } ?>
<title><?php echo $shortname; ?> <?php echo $pagename; ?></title>

<script type="text/javascript">
var andSoItBegins = (new Date()).getTime();
</script>

<link rel="shortcut icon" href="<?php echo $path; ?>/habboweb/63_1d5d8853040f30be0cc82355679bba7c/3389/web-gallery/v2/favicon.ico" type="image/vnd.microsoft.icon" />

<script src="<?php echo $path; ?>/habboweb/63_1d5d8853040f30be0cc82355679bba7c/3389/web-gallery/static/js/libs2.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/habboweb/63_1d5d8853040f30be0cc82355679bba7c/3389/web-gallery/static/js/visual.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/habboweb/63_1d5d8853040f30be0cc82355679bba7c/3389/web-gallery/static/js/libs.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/habboweb/63_1d5d8853040f30be0cc82355679bba7c/3389/web-gallery/static/js/common.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/habboweb/63_1d5d8853040f30be0cc82355679bba7c/3389/web-gallery/static/js/fullcontent.js" type="text/javascript"></script>
<?php if($pageid == "10"){ ?>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/rooms.css" type="text/css">
<?php } ?>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/style.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/buttons.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/boxes.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/tooltips.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/changepassword.css" type="text/css" />
<?php if($pageid == "5"){ ?>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/newcredits.css" type="text/css" />
<script src="<?php echo $path; ?>/web-gallery/static/js/newcredits.js" type="text/javascript"></script>
<?php } ?>

<?php if($pageid == "6"){ ?>
<script src="./web-gallery/static/js/habboclub.js" type="text/javascript"></script>
<?php } ?>

<link rel="alternate" type="application/rss+xml" title="<?php $shortname; ?> RSS" href="<?php echo $path; ?>/rss.php"/>

<script type="text/javascript">
document.habboLoggedIn = true;
var habboName = "<?php echo $name; ?>";
var habboStaticFilePath = "<?php echo $path; ?>/habboweb/63_1d5d8853040f30be0cc82355679bba7c/3389/web-gallery";
var habboImagerUrl = "<?php echo $path; ?>/habbo-imaging/";
var habboPartner = "<?php echo $path; ?>";
window.name = "habboMain";
if (typeof HabboClient != "undefined") { HabboClient.windowName = "uberClientWnd"; }
</script>

<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/newcredits.css" type="text/css" />
<script src="<?php echo $path; ?>/web-gallery/static/js/newcredits.js" type="text/javascript"></script>

<?php if($pageid == "10b" or $pageid == "forum"){ ?>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/myhabbo.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/skins.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/dialogs.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/buttons.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/control.textarea.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/boxes.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/myhabbo.css" type="text/css" />

<script src="<?php echo $path; ?>/web-gallery/static/js/homeview.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/homeauth.js" type="text/javascript"></script>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/group.css" type="text/css" />
<style type="text/css">

    #playground, #playground-outer {
	    width: 752px;
	    height: 1360px;
    }

<?php if(isset($groupid)){ ?>

    #playground, #playground-outer {
	    width: 922px;
	    height: 1360px;
    }

<?php } elseif(getHCDays($user_row['id'])){ ?>

    #playground, #playground-outer {
	    width: 922px;
	    height: 1360px;
    }

<?php } else { ?>

    #playground, #playground-outer {
	    width: 752px;
	    height: 1360px;
    }

<?php } ?>


</style>

<script type="text/javascript">
document.observe("dom:loaded", function() { initView(55918, 1478728); });
</script>

<link href="<?php echo $path; ?>/web-gallery/styles/discussions.css" type="text/css" rel="stylesheet"/>
<?php } ?>

<?php if($body_id == "home" || empty($body_id)){ ?>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/welcome.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/personal.css" type="text/css" />

<script src="<?php echo $path; ?>/web-gallery/static/js/group.js" type="text/javascript"></script>

<script src="<?php echo $path; ?>/web-gallery/static/js/rooms.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/moredata.js" type="text/javascript"></script>
<?php } ?>

<?php if($pageid == "1" || $pageid == "7"){ ?>
<script src="<?php echo $path; ?>/web-gallery/static/js/habboclub.js" type="text/javascript"></script>
<?php } ?>

<?php if($body_id == "profile"){ ?>
<script src="<?php echo $path; ?>/web-gallery/static/js/settings.js" type="text/javascript"></script>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/settings.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/friendmanagement.css" type="text/css" />
<?php } ?>

<?php if($pageid == 1){ ?>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/minimail.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/control.textarea.css" type="text/css" />
<script src="<?php echo $path; ?>/web-gallery/static/js/minimail.js" type="text/javascript"></script>
<?php } ?>

<!--[if lt IE 8]>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/ie.css" type="text/css" />
<![endif]-->
<!--[if lt IE 7]>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/ie6.css" type="text/css" />
<script src="<?php echo $path; ?>/web-gallery/static/js/pngfix.js" type="text/javascript"></script>
<script type="text/javascript">
try { document.execCommand('BackgroundImageCache', false, true); } catch(e) {}
</script>

<style type="text/css">
body { behavior: url(web-gallery/csshover.htc); }
</style>
<![endif]-->
<meta name="build" content="PRODUCTION-BUILD3998 - 19.08.2015 08:50 - br - wulles" />
</head>