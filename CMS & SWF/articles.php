<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$body_id = "news";
$pageid = "11";

$news_id = FilterText($_GET['web-articles-id']);
$main_sql = mysql_query("SELECT * FROM cms_news WHERE id = '".$news_id."'") or die(mysql_error());
$article_exists = mysql_num_rows($main_sql);

if($article_exists == "1"){
	$news = mysql_fetch_assoc($main_sql);
	$pagename = "Noticias - ".HoloText($news['title'])."";
} else {
	$main_sql = mysql_query("SELECT * FROM cms_news ORDER BY ID DESC") or die(mysql_error());
	$news = mysql_fetch_assoc($main_sql);
	$news_id = $news['id'];
	$pagename = "Noticias - ".HoloText($news['title'])."";
}


require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>
<div id="container">
<div id="content">
<div id="column1" class="column">
<div class="habblet-container ">
<div class="cbb clearfix orange">
	<h2 class="title">Índice de Notícias</h2>
<div id="article-archive">
<?php

if ($news_id > 0)
{
 for ($i = 0; $i < 7; $i++)
 {
 $sectionName = "";
 $sectionCutoffMax = 0;
 $sectionCutoffMin = 0;
 
switch ($i)
 {
 case 0:
 
$sectionName = "Hoje";
 $sectionCutoffMax = time();
 $sectionCutoffMin = time() - 86400;
 break;
 
case 1:
 
$sectionName = "Ontem";
 $sectionCutoffMax = time() - 86400;
 $sectionCutoffMin = time() - 172800;
 break;
 
case 2: 
 
$sectionName = "Essa Semana";
 $sectionCutoffMax = time() - 172800;
 $sectionCutoffMin = time() - 604800;
 break;
 
case 3:
 
$sectionName = "Semana Passada";
 $sectionCutoffMax = time() - 604800;
 $sectionCutoffMin = time() - 1209600;
 break;
 
case 4:
 
$sectionName = "Esse Mês";
 $sectionCutoffMax = time() - 1209600;
 $sectionCutoffMin = time() - 2592000;
 break;
 
case 5:
 
$sectionName = "Mês Passado";
 $sectionCutoffMax = time() - 2592000;
 $sectionCutoffMin = time() - 5184000;
 break;
               

            case 6:
 
$sectionName = "Outros Meses";
 $sectionCutoffMax = time() - 3592000;
 $sectionCutoffMin = time() - 5684000;
 break;

 }

 $sql = mysql_query("SELECT * FROM cms_news WHERE published >= " . $sectionCutoffMin . " AND published <= " . $sectionCutoffMax .  " ORDER BY published DESC"); 
 if(mysql_num_rows($sql) > 0){
?>

 <?php echo "<h2>"; echo $sectionName; echo "</h2><ul>"; ?>
<?php while($row = mysql_fetch_assoc($sql)){ ?>
 <li><?php if($news_id !== $row['id']){ echo"<a href=\"".$path."/articles/".$row['id']."\">"; } ?>
 <?php echo $row['title']; ?> »</a> 
 <?php if($news_id !== $row['id']){ echo"</a>"; } ?></li>
 
<?php } ?>

</ul>

<?php } } } ?>

<ul>
 <li>
 <a href="<?php echo $path; ?>/articles/" class="article">Mais Notícias »</a>
 </li>
</ul>
</div>
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>

<div id="column2" class="column">
<div class="habblet-container ">
<div class="cbb clearfix notitle ">

<div id="article-wrapper">
	<h2><?php echo HoloText($news['title']); ?></h2>
	<div class="article-meta">Postado em <b><?php echo date('d-m-Y', $news['published']); ?></b> por <b><?php echo $news['author']; ?></b></div>
	<div class="article-body">
	<p><?php echo (HoloText($news['shortstory'], true)); ?></p></div></div>
</div>


<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>

<?php require_once('./templates/community_footer.php'); ?>