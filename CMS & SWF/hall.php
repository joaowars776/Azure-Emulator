<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Hall da Fama";
$pageid = "1000";
$body_id = "credits";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<div id="container">
<div id="column1" class="column">

<div id="container">
<div id="content" style="position: relative" class="clearfix">


<div id="column1" class="column">
<div class="habblet-container ">
<div class="cbb clearfix red ">
<h2 class="title"><span style="float: left;">O que é Hall da Fama?</span></h2>
		<div class="box-content">
 <center style="font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; background-color: rgb(255, 255, 255);">
   <p>
    <img src="/web-gallery/images/hdf.gif" /></p>
  </center>
<p><p><span style="font-size: 11px;"><span style="color: #3366ff;"><strong>Hall da Fama</strong></span> foi criado com o intuito de promover os usuários que mais se sobressaem nas atividades proporcionadas pela Equipe Habbluz. A única maneira de entrar no <em>TOP</em> de qualquer categoria, é <strong>participando ativamente</strong> dessas atividades, sejam elas promoções ou eventos do dia-á-dia promomovidos pela MODeração.</p>
<div>
<p><span style="font-size: 11px;">Quando falamos sobre as promoções, tanto faz as mesmas serem de quarto, pixel-art, foto, aúdio e/ou vídeo, quarto, texto e/ou desenho, colunas semanais, enigmas e/ou atividades especiais, a mesma só irá lhe propor um ponto nessa categoria, se possuir o símbolo indicador, sendo o mesmo apresentado no fim da notícia, que será de escolha do autor do mesmo. <em></em>.</p>
<div>
<p><span style="color: #3366ff;"><strong><span style="font-size: 11px;"><span style="font-size: 11px;">Vantagens de estar no Hall:</strong></span></p>
<div>
<p><span style="color: #3366ff;"><strong><span style="font-size: 11px;">»</strong></span><span style="font-size: 11px;"> Estar no Topo do Hall torna você uma celebridade no hotel;</em><br /><img style="float: right;"  alt="" /><span style="color: #3366ff;"><strong>»</strong></span> Cada usuário portará seu próprio perfil no hall. <br /><i>(No qual detalha todas as atividades ganhas, além de um quadro contando todos os emblemas conquistados)</i></em></p>
<div>
<p><span style="color: #3366ff;"><strong><span style="font-size: 11px;">Premiação:</strong></span></p>
<div>
<p><span style="color: #3366ff;"><strong><span style="font-size: 11px;">»</strong></span><span style="font-size: 11px;"> A premiação em eventos consiste em um emblema do dia em questão, 1 ponto na categoria "Eventos" do Hall da Fama. Confira nas notícias os emblemas que serão utilizados na <em>premiação dos eventos este mês</em>. Qualquer dúvida, procurar os membros da <strong>Equipe Habbluz</strong>.
<center><p><img src="http://i.imgur.com/dWDL8jb.gif" /><img src="http://i.imgur.com/ENBIgbk.gif" /><img src="http://i.imgur.com/1DyoJXb.gif" /><img src="http://i.imgur.com/tQstH5K.gif" /><img src="http://i.imgur.com/Xkqz1nE.gif" /><img src="http://i.imgur.com/zNAHyjN.gif" /><img src="http://i.imgur.com/CP2m6yW.gif" /><img src="http://i.imgur.com/y4iTJlg.gif" /></p></center>

</div></div>
		</div>
		</div>
		<br/>
		<br/></div>
		</div>
</div>
</div>
</div>


<div id="column2" class="column">
<div class="habblet-container ">
<div class="cbb clearfix orange ">
<h2 class="title">
<span style="float: left;">TOP Eventos</span>
</h2>
<?php
$rank = "SELECT * FROM rankevento ORDER BY pontos DESC LIMIT 4";
$resranking = mysql_query($rank) or die(mysql_error());
$verifica = mysql_num_rows(mysql_query("SELECT * FROM rankevento"));
if($verifica!=0){
$i=1;
while($row=mysql_fetch_array($resranking)){

$mostralook = mysql_query(sprintf("SELECT look FROM users WHERE username = '%s'", $row[nome]));
$mostralook = mysql_fetch_assoc($mostralook);
$mostralook = $mostralook['look']; 

if(IsEven($i)){
	$even = "even";
} else {
	$even = "odd";
}

if($row['online'] == "1"){
	$image = "online_anim_big";
} else {
	$image = "offline_big";
}
?>
<li class="<?php echo $even; ?>">
<table width="452" border="0">
  <tr>
    <td width="50" valign="top"><div style="background:url(http://habbo.com.tr/habbo-imaging/avatarimage?figure=<?php echo $mostralook; ?>&size=b&action=wav&direction=3&head_direction=3&gesture=sml) -10px -14px; width:50px; height:60px;"></div></td>
    <td width="392" valign="top"><br /><span style="font-size: 14px;"><b><font size="2">Usuario:</font></b><span style="font-size: 12px;"> <?php echo $row['nome']; ?>
     <br/>
	 <b><b><font size="2">Pontos:</font></b><span style="font-size: 16px;"> <font size="2"><font color="#B25900"><?php echo $row['pontos']; ?></font></font><br />
      <div>
	  </td>
  </tr>
</table>
<?php
$i++;
}
}else{
echo("<table width='288' border='0'>
  <tr>
    <td width='57'><img src='http://i.imgur.com/TF4HY8u.gif' width='57' height='85' /></td>
    <td width='221' valign='top'>
      <hr style='border:#ccc 1px dotted;' />
      <a href='/client'>Ganhe eventos no Hotel para aparecer aqui!</a></td>
  </tr>
</table>
");
}
?>
</div>
</div>
</div>


<div id="column2" class="column">
<div class="habblet-container ">
<div class="cbb clearfix blue ">
<h2 class="title">
<span style="float: left;">TOP Promoções</span>
</h2>

<?php
$rank = "SELECT * FROM rankcampanha ORDER BY pontos DESC LIMIT 3";
$resranking = mysql_query($rank) or die(mysql_error());
$verifica = mysql_num_rows(mysql_query("SELECT * FROM rankcampanha"));
if($verifica!=0){
$i=1;
while($row=mysql_fetch_array($resranking)){

$mostralook = mysql_query(sprintf("SELECT look FROM users WHERE username = '%s'", $row[nome]));
$mostralook = mysql_fetch_assoc($mostralook);
$mostralook = $mostralook['look']; 

if(IsEven($i)){
	$even = "even";
} else {
	$even = "odd";
}

if($row['online'] == "1"){
	$image = "online_anim_big";
} else {
	$image = "offline_big";
}
?>
<li class="<?php echo $even; ?>">
<table width="452" border="0">
  <tr>
    <td width="50" valign="top"><div style="background:url(http://habbo.com.tr/habbo-imaging/avatarimage?figure=<?php echo $mostralook; ?>&size=b&action=wav&direction=3&head_direction=3&gesture=sml) -10px -14px; width:50px; height:60px;"></div></td>
    <td width="392" valign="top"><span style="font-size: 14px;"><b><font size="2">Usuario:</font></b><span style="font-size: 12px;"> <?php echo $row['nome']; ?><br />
     <br/>
	 <b><font size="2">Pontos:</font></b><span style="font-size: 16px;"> <font color="#B25900"><?php echo $row['pontos']; ?></font><br />
      <div></td>
  </tr>
</table>
<?php
$i++;
}
}else{
echo("<table width='288' border='0'>
  <tr>
    <td width='57'><img src='http://i.imgur.com/TF4HY8u.gif' width='57' height='85' /></td>
    <td width='221' valign='top'>
      <hr style='border:#ccc 1px dotted;' />
      <a href='/promosativas.php'> Ganhe promoções no hotel para aparecer aqui!</a></td>
  </tr>
</table>
");
}
?>
</div>
</div>
</div>


<script type="text/javascript">
			document.observe("dom:loaded", function() { PromoSlideShow.init(); });
		</script>
<script type="text/javascript">
			HabboView.run();
		</script>
		</div>
	</div>
</div>
</div>
<?php require_once('./templates/community_footer.php'); ?>