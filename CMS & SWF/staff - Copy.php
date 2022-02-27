<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Equipe";
$pageid = "13";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>

<div id="container"><div id="content" style="position: relative" class="clearfix"><div id="column1" class="column"><div class="habblet-container">
<div class="cbb clearfix green">
<h2 class="title">Nossa Equipe</h2>
<div class="box-content">
<p>Nosso hotel é mantido por uma equipe criteriosamente selecionada para lhes proporcionar o melhor entretenimento e segurança. Eles estão aqui para certificar que tudo estará funcionando corretamente e de acordo com a nossa etiqueta.</p>
<p>Portanto, caso algum jogador do hotel afirme ser um membro de nossa equipe e não porte o emblema staff, entre em contato com a Equipe de Moderação, ou então emita uma denúncia sobre o mesmo.</p>
</div>
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script></div><div id="column2" class="column"><div class="habblet-container ">
<div class="cbb clearfix settings ">
<h2 class="title">Navegador</h2>
<div class="habblet box-content" style="align:left">
<b>Cargos no Hotel</b><br/><br/>
- <a href='comunidade/criadores'>Criadores</a><br/>
- <a href='comunidade/gerentes'>Gerentes</a><br/>
- <a href='comunidade/administradores'>Administradores</a> <br/>
- <a href='comunidade/moderadores'>Moderadores</a> <br/>
- <a href='comunidade/embaixadores'>Embaixadores</a> <br/>
</div>
</div>
</div>
<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
</div>
<script type="text/javascript">
			HabboView.run();
		</script>
</div>
<?php require_once('./templates/community_footer.php'); ?>
</div>
</div>
</div>
</body>