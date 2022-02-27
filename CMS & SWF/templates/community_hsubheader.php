<?php if (!defined("IN_HOLOCMS")) { header("Location: $path"); exit; }

if(empty($pagename) && isset($searchname)){
$pagename = $searchname;
} elseif(empty($pagename)){
$pagename = "Home";
}

?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
        "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
	<base href="<?php echo $path; ?>/">
	<?php header("Content-Type: text/html; charset=iso-8859-1",true); ?>
	<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
	<meta http-equiv="content-type" content="text/html; charset=utf-8" />
	<title><?php echo $shortname; ?>: <?php echo HoloText($pagename); ?> </title>

<script type="text/javascript">
var andSoItBegins = (new Date()).getTime();
</script>

<link rel="shortcut icon" href="<?php echo $path; ?>/web-gallery/v2/images/favicon.ico" type="image/vnd.microsoft.icon" />
<script src="<?php echo $path; ?>/web-gallery/static/js/visual.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/libs.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/common.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/fullcontent.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/libs2.js" type="text/javascript"></script>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/style.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/buttons.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/boxes.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/tooltips.css" type="text/css" />
<link rel="alternate" type="application/rss+xml" title="<?php $shortname; ?>: RSS" href="./rss.php"/>

<script type="text/javascript">
document.habboLoggedIn = true;
var habboName = "<?php echo $name; ?>";
var habboReqPath = "";
var habboStaticFilePath = "<?php echo $path; ?>/web-gallery";
var habboImagerUrl = "http://www.habbo.es/habbo-imaging/";
var habboPartner = "Meth0d.org";
window.name = "habboMain";

</script>

<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/myhabbo.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/skins.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/dialogs.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/buttons.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/control.textarea.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/styles/myhabbo/boxes.css" type="text/css" />
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/myhabbo.css" type="text/css" />
<link href="<?php echo $path; ?>/web-gallery/styles/myhabbo/assets.css" type="text/css" rel="stylesheet" />
<script src="<?php echo $path; ?>/web-gallery/static/js/homeview.js" type="text/javascript"></script>
<script src="<?php echo $path; ?>/web-gallery/static/js/homeauth.js" type="text/javascript"></script>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/v2/styles/group.css" type="text/css" />
<style type="text/css">

<?php if(isset($groupid)){ ?>

    #playground, #playground-outer {
	    width: 922px;
	    height: 1360px;
    }

<?php } elseif($hc > 0){ ?>

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




<script src="<?php echo $path; ?>/web-gallery/static/js/homeedit.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript">
<?php if(isset($groupid)){ $xid = $groupid; } else { $xid = $user_row['id']; } ?>
document.observe("dom:loaded", function() { initView(<?php echo $xid . "," . $xid; ?>); });
function isElementLimitReached() {
	if (getElementCount() >= 200) {
		showHabboHomeMessageBox("Erro", "Vocês colocaram o número de itens maxima nesta página. Remove um adesivo nota, ou widget para colocar este item.", "Fechar");
		return true;
	}
	return false;
}

function cancelEditing(expired) {
	<?php if(!isset($groupid)){ ?>
	location.replace("<?php echo $path; ?>/home/<?php echo $searchname; ?>" + (expired ? "?expired=true" : ""));
	<?php } else { ?>
	location.replace("<?php echo $path; ?>/groups/<?php echo $groupid; ?>" + (expired ? "?expired=true" : ""));
	<?php } ?>
}

function getSaveEditingActionName(){
	return '../myhabbo/savehome.php';
}

function showEditErrorDialog() {
	var closeEditErrorDialog = function(e) { if (e) { Event.stop(e); } Element.remove($("myhabbo-error")); Overlay.hide(); }
	var dialog = Dialog.createDialog("myhabbo-error", "", false, false, false, closeEditErrorDialog);
	Dialog.setDialogBody(dialog, '<p>Falha! Por favor, tente novamente em alguns minutos.</p><p><a href="#" class="new-button" id="myhabbo-error-close"><b>Fechar</b><i></i></a></p><div class="clear"></div>');
	Event.observe($("myhabbo-error-close"), "click", closeEditErrorDialog);
	Dialog.moveDialogToCenter(dialog);
	Dialog.makeDialogDraggable(dialog);
}


function showSaveOverlay() {
	var invalidPos = getElementsInInvalidPositions();
	if (invalidPos.length > 0) {
		showHabboHomeMessageBox("&iexcl;Lo Sentimos!", "Desculpe, mas eu não posso colocar nada aqui;. Feche esta janela para continuar a editar a sua Home.", "Fechar");
		$A(invalidPos).each(function(el) { Effect.Pulsate(el); });
		return false;
	} else {
		Overlay.show(null,'Guardar');
		return true;
	}
}
</script>

<meta name="description" content="<?php echo $shortname; ?> Hotel: É amigo, divirtam-se e torne-se conhecido." />
<meta name="keywords" content="<?php echo $shortname; ?> hotel de mundo virtual, a rede social, livre, comunidade, caráter, bate-papo, online, teen, roleplaying, participar de grupos sociais, fóruns, seguros, jogar, jogos, amigos, adolescentes, raros, mobili raros, colecionáveis, criar, coleta, ligue, mobili, móveis, mobiliário, design de interiores, compartilhamento, de expressão, placas, sair, música, celebridades, visitas de celebridades, celebridades, jogos online, jogos multiplayer, multiplayer" />

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
body { behavior: url(<?php echo $path; ?>/web-gallery/csshover.htc); }
</style>
<![endif]-->
<meta name="build" content="21.0.53 - HoloCMS BETA v4.6 UBER Homes" />
</head>