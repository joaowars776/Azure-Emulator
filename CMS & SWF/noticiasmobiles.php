<link rel="stylesheet" href="./web-gallery/static/styles/lightweightmepage.css" type="text/css" />
<script src="./web-gallery/static/js/lightweightmepage.js" type="text/javascript"></script>

<div id="promo-box">
<div id="promo-bullets"></div>

<?php
$news1_5 = mysql_query("SELECT * FROM cms_news ORDER BY id DESC LIMIT 6") or die(mysql_error());
?>

<?php $i = 0; while($news = mysql_fetch_assoc($news1_5)){ $i++; ?>

        <div class="promo-container" style="background-image: url(<?php echo $news['image']; ?>)<?php if($i != '1'){ ?>; display: none<?php } ?>">
<div class="promo-content-container">
<div class="promo-content">
<div class="title"><?php echo $news['title']; ?></div>
<div class="body"><?php echo $news['longstory']; ?></div>
</div>
</div>
<div class="promo-link-container">
<div class="enter-hotel-btn">
<div class="open enter-btn">
<a style="padding: 0 8px 0 19px;" href="/articles/<?php echo $news['id'] ?>">Saiba Mais</a><b></b>
</div>
</div>
</div>
</div>

		<?php } ?>

 </div>

<script type="text/javascript">    document.observe("dom:loaded", function() { PromoSlideShow.init(); });</script>
