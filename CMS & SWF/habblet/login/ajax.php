<!--[if lt IE 7]>      <html class="no-js lt-ie9 lt-ie8 lt-ie7"> <![endif]-->
<!--[if IE 7]>         <html class="no-js lt-ie9 lt-ie8"> <![endif]-->
<!--[if IE 8]>         <html class="no-js lt-ie9"> <![endif]-->
<!--[if gt IE 8]><!--> <html class="no-js"> <!--<![endif]-->
<head>

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="pt-br" lang="pt-br" xmlns:og="http://opengraphprotocol.org/schema/" xmlns:fb="http://www.facebook.com/2008/fbml"><head> 
<?php header("Content-Type: text/html; charset=iso-8859-1",true); ?>
<BODY background-color="#ffd700">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title><?php echo $sitename; ?> - Hotel</title>
    <meta name="viewport" content="width=device-width">
    <script>
        var andSoItBegins = (new Date()).getTime();
        var habboPageInitQueue = [];
        var habboStaticFilePath = "<?php echo $path; ?>/habboweb/63_1d5d8853040f30be0cc82355679bba7c/3389/web-gallery";
    </script>
    <link rel="shortcut icon" href="<?php echo $path; ?>/habboweb/63_1d5d8853040f30be0cc82355679bba7c/3389/web-gallery/v2/favicon.ico" type="image/vnd.microsoft.icon">
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Ubuntu:400,700,400italic,700italic">
<link rel="stylesheet" href="<?php echo $path; ?>/habboweb/63_1d5d8853040f30be0cc82355679bba7c/3389/web-gallery/static/styles/v3_landing.css" type="text/css" />
<script src="<?php echo $path; ?>/habblet/js/v3_landing_top2.js" type="text/javascript"></script>

        <meta name="google-site-verification" content="9ZMc5CJdR2sKq3Ev3lagZHr5IFQW_AsoxMyRPdNybaQ">

    <link rel="alternate" href="https://www.habbo.de" hreflang="de">
    <link rel="alternate" href="https://www.habbo.com" hreflang="en">
    <link rel="alternate" href="https://www.habbo.es" hreflang="es">
    <link rel="alternate" href="https://www.habbo.fi" hreflang="fi">
    <link rel="alternate" href="https://www.habbo.fr" hreflang="fr">
    <link rel="alternate" href="https://www.habbo.it" hreflang="it">
    <link rel="alternate" href="https://www.habbo.nl" hreflang="nl">
    <link rel="alternate" href="https://www.habbo.com.br" hreflang="pt">
    <link rel="alternate" href="https://www.habbo.com.tr" hreflang="tr">

	<script type="text/javascript">
			habboPageInitQueue.push(function() {
				$(function() {
					$("#eu_cookie_policy_close").on('click', function() {
						$("#eu_cookie_policy").fadeOut();
						setCookie("close_clone", "close", 1); 
					});
					if(getCookie("close_clone") != "close")
						$("#eu_cookie_policy").show();
				});	
			});
		</script>

</head>
<body>

<div id="overlay"></div>

<div id="change-password-form" class="overlay-dialog" style="display: none;">
    <div id="change-password-form-container" class="clearfix form-container">
        <h2 id="change-password-form-title" class="bottom-border">Esqueceu sua senha?</h2>
        <div id="change-password-form-content" style="display: none;">
		Caso você tenha esquecido sua senha, envie um email para <a href="mailto:lukiface@icloud.com" taget="_new">lukiface@icloud.com</a> com o seu Nome de Usuário e seu Email, se for comprovado que você é o dono da conta, sua nova senha será enviada para você em 1 hora.
            <div class="change-password-buttons">
                <a href="" id="change-password-cancel-link">Cancelar</a>            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    function initChangePasswordForm() {
        ChangePassword.init();
    }
    if (window.HabboView) {
        HabboView.add(initChangePasswordForm);
    } else if (window.habboPageInitQueue) {
        habboPageInitQueue.push(initChangePasswordForm);
    }
</script>


<header>

<div id="eu_cookie_policy" class="orange-notifier eu_cookie_policy_v3" style="display: block;"><div><a class="close" id="eu_cookie_policy_close"><u>Fechar</u></a><span>
Acesse sua conta e participe de promoções e eventos! Chame seus amigos e <b>nunca revele seus dados</b> para outros jogadores.</span></div></div> 


    <div id="border-left"></div>
    <div id="border-right"></div>
	<?php
	if(isset($login_fehler))
	{
	?>
    <div id="login-errors"><?php echo $login_fehler; ?></div>
	<?php
	}
	?>

<div id="login-form-container">
    <a href="#home" id="habbo-logo"></a>

    <form method="post">


    <div id="login-columns">
        <div id="login-column-1">
            <label for="credentials-email">Nome</label>
            <input tabindex="2" type="text" name="credentials_username" id="credentials-email" value="<?php if(isset($_COOKIE['username'])) echo $_COOKIE['username']; ?>">
            <input tabindex="5" type="checkbox" name="_login_remember_me" id="credentials-remember-me">
            <label for="credentials-remember-me" class="sub-label">Mantenha-me conectado</label>
        </div>

        <div id="login-column-2">
            <label for="credentials-password">Senha</label>
            <input tabindex="3" type="password" name="credentials.password" id="credentials-password">
            <a href="#" id="forgot-password" class="sub-label">Esqueceu a senha?</a>
        </div>

		</div>

        <div id="login-column-3">
            <input type="submit" value="Login" style="margin: -10000px; position: absolute;">
            <a href="#" tabindex="4" class="button" id="credentials-submit"><b></b><span>Login</span></a>

        </div>

    </div>
</form>
</div>

<script>
    habboPageInitQueue.push(function() {
        if (!LandingPage.focusForced) {
            LandingPage.fieldFocus('credentials-email');
        }
    });
</script>
    <div id="alerts">
<script type="text/javascript">
    document.cookie = "habbotestcookie=supported";
    var cookiesEnabled = document.cookie.indexOf("habbotestcookie") != -1;
    if (cookiesEnabled) {
        var date = new Date();
        date.setTime(date.getTime()-24*60*60*1000);
        document.cookie="habbotestcookie=supported; expires="+date.toGMTString();
    } else {
</script>
    </div>
    <div id="top-bar-triangle"></div>
    <div id="top-bar-triangle-border"></div>
</header>


<div id="content">
    <ul>
        <li id="home-anchor" class="home-anchor-day" >
            <div id="welcome">
                <a href="#registration" class="button large" id="join-now-button"><b></b><span>Entre hoje</span><span class="sub">(É grátis)</span></a>
                <div id="slogan">
                    <h1>Bem-vindo ao <?php echo $sitename; ?>,</h1>
                    <p>um lugar divertido com gente incrível.</p>
                    <p><a id="tell-me-more-link" href="#">Saiba Mais!</a></p>
                </div>
            </div>
            <div id="carousel">
                <div id="image1" style="background-image: url(//habboo-a.akamaihd.net/c_images/Comufy-Social/frontpage_img_1.png);"></div>
                <div id="image2" style="background-image: url(//habboo-a.akamaihd.net/c_images/Comufy-Social/frontpage_img_2.png);"></div>
                <div id="image3" style="background-image: url(//habboo-a.akamaihd.net/c_images/Comufy-Social/frontpage_iosAnd_BR.png);"></div>
                <div id="tell-me-more">O <?php echo $sitename; ?> Hotel é um mundo virtual para jogadores maiores de 13 anos, onde você pode criar seu próprio personagem e projetar seu Quarto do jeito que quiser. Você poderá conhecer novos amigos, organizar festas, cuidar de mascotes virtuais, jogar e criar seus próprios jogos e completar Tarefas. Clique em "Entre hoje" para começar!</div>
			</div>

            <div id="floaters"></div>
        </li>


        <li id="registration-anchor">

<div id="registration-form">
    <div id="registration-form-header">
     <h2>Seu ID</h2>
        <p>Preencha estes detalhes para começar:</p>
    </div>
    <div id="registration-form-main">
        <form id="register-new-user" autocomplete="off">
        <input type="hidden" name="next" value="">
        <div id="registration-form-main-left">
            <label for="registration-birthday">Data de nascimento</label>
            <label for="registration-birthday" class="details">Usaremos isso para restaurar sua conta caso você perca o acesso. Sua data de nascimento nunca será compartilhada publicamente.</label>
<div id="registration-birthday">
<select name="registrationBean.day" id="registrationBean_day" class="dateselector"><option value="">Dia</option><option value="1">1</option><option value="2">2</option><option value="3">3</option><option value="4">4</option><option value="5">5</option><option value="6">6</option><option value="7">7</option><option value="8">8</option><option value="9">9</option><option value="10">10</option><option value="11">11</option><option value="12">12</option><option value="13">13</option><option value="14">14</option><option value="15">15</option><option value="16">16</option><option value="17">17</option><option value="18">18</option><option value="19">19</option><option value="20">20</option><option value="21">21</option><option value="22">22</option><option value="23">23</option><option value="24">24</option><option value="25">25</option><option value="26">26</option><option value="27">27</option><option value="28">28</option><option value="29">29</option><option value="30">30</option><option value="31">31</option></select> <select name="registrationBean.month" id="registrationBean_month" class="dateselector"><option value="">Mês</option><option value="1">Janeiro</option><option value="2">Fevereiro</option><option value="3">Março</option><option value="4">Abril</option><option value="5">Maio</option><option value="6">Junho</option><option value="7">Julho</option><option value="8">Agosto</option><option value="9">Setembro</option><option value="10">Outubro</option><option value="11">Novembro</option><option value="12">Dezembro</option></select> <select name="registrationBean.year" id="registrationBean_year" class="dateselector"><option value="">Ano</option><option value="2007">2007</option><option value="2006">2006</option><option value="2005">2005</option><option value="2004">2004</option><option value="2003">2003</option><option value="2002">2002</option><option value="2001">2001</option><option value="2000">2000</option><option value="1999">1999</option><option value="1998">1998</option><option value="1997">1997</option><option value="1996">1996</option><option value="1995">1995</option><option value="1994">1994</option><option value="1993">1993</option><option value="1992">1992</option><option value="1991">1991</option><option value="1990">1990</option><option value="1989">1989</option><option value="1988">1988</option><option value="1987">1987</option><option value="1986">1986</option><option value="1985">1985</option><option value="1984">1984</option><option value="1983">1983</option><option value="1982">1982</option><option value="1981">1981</option><option value="1980">1980</option><option value="1979">1979</option><option value="1978">1978</option><option value="1977">1977</option><option value="1976">1976</option><option value="1975">1975</option><option value="1974">1974</option><option value="1973">1973</option><option value="1972">1972</option><option value="1971">1971</option><option value="1970">1970</option><option value="1969">1969</option><option value="1968">1968</option><option value="1967">1967</option><option value="1966">1966</option><option value="1965">1965</option><option value="1964">1964</option><option value="1963">1963</option><option value="1962">1962</option><option value="1961">1961</option><option value="1960">1960</option><option value="1959">1959</option><option value="1958">1958</option><option value="1957">1957</option><option value="1956">1956</option><option value="1955">1955</option><option value="1954">1954</option><option value="1953">1953</option><option value="1952">1952</option><option value="1951">1951</option><option value="1950">1950</option><option value="1949">1949</option><option value="1948">1948</option><option value="1947">1947</option><option value="1946">1946</option><option value="1945">1945</option><option value="1944">1944</option><option value="1943">1943</option><option value="1942">1942</option><option value="1941">1941</option><option value="1940">1940</option><option value="1939">1939</option><option value="1938">1938</option><option value="1937">1937</option><option value="1936">1936</option><option value="1935">1935</option><option value="1934">1934</option><option value="1933">1933</option><option value="1932">1932</option><option value="1931">1931</option><option value="1930">1930</option><option value="1929">1929</option><option value="1928">1928</option><option value="1927">1927</option><option value="1926">1926</option><option value="1925">1925</option><option value="1924">1924</option><option value="1923">1923</option><option value="1922">1922</option><option value="1921">1921</option><option value="1920">1920</option><option value="1919">1919</option><option value="1918">1918</option><option value="1917">1917</option><option value="1916">1916</option><option value="1915">1915</option><option value="1914">1914</option><option value="1913">1913</option><option value="1912">1912</option><option value="1911">1911</option><option value="1910">1910</option><option value="1909">1909</option><option value="1908">1908</option><option value="1907">1907</option><option value="1906">1906</option><option value="1905">1905</option><option value="1904">1904</option><option value="1903">1903</option><option value="1902">1902</option><option value="1901">1901</option><option value="1900">1900</option></select>             </div>



			   <label for="registration-username">Escolha seu nome</label>
            <label for="registration-username" class="details">Você ultilizará este nome futuramente para acessar a sua conta no hotel.</label>
            <input type="text" name="registrationBean_username" id="registration-username" value="">



</span>
        </div>

        <div id="registration-form-main-right">




			<label for="registration-password">Escolha sua senha</label>
                <label for="registration-password" class="details">Senhas precisam ter no mínimo<b>6 caracteres</b>, incluíndo<b> letras e números</b></b></b></label>
                <input type="password" name="registrationBean_password" id="registration-password" maxlength="32" value="">

				 <label for="registration-password2">Reescreva sua senha</label>
                <label for="registration-password2" class="details">Digite a senha novamente.</b></b></label>
                <input type="password" name="registrationBean_password2" id="registration-password2" maxlength="32" value="">



</p>

		</b>
              <label for="registration-email">Coloque um e-mail</label>
            <label for="registration-email" class="details">Você deve utilizar um email real.</label>
            <input type="email" name="registrationBean_email" id="registration-email" value="">




            <p class="checkbox-container">

               <br>
            <a href="#" class="button large not-so-large register-submit"><b></b><span>Pronto!</span></a>
</p>

</div>
        </form>
    </div>
</div>
<div id="magnifying-glass"></div>
            <div id="sail"></div>
        </li>
    </ul>
</div>

<footer>
        <div id="partner-logo"><a href="https://help.habbo.com.br/entries/390961-O-que-%C3%A9-o-Habbo-" style="background-image: url('//habboo-a.akamaihd.net/c_images/Landingpage_partner_logos/recomendado.png')"></a></div>
    <div id="age-recommendation"></div>

    <div id="footer-content" class="partner-logo-present">
        <div id="footer"><a href="mailto: advertising@sulake.com?" taget="_new">Anuncie</a> / <a href="https://help.habbo.com.br" target="_new">Contate-nos</a> / <a href="https://help.habbo.com.br/forums" target="_new">FAQs</a> / <a href="http://www.sulake.com" target="_new">Sulake</a> / <a href="https://help.habbo.com.br/entries/20477982-termos-e-condicoes" target="_new">Termos e Condições</a> / <a href="https://help.habbo.com.br/entries/390888-politica-de-privacidade" target="_new">Política de Privacidade</a> / <a href="https://help.habbo.com.br/forums/311751-informacoes-para-os-pais" target="_new">Guia dos Pais</a> / <a href="http://www.habbo.com.br/safety/habbo_way" target="_new">Habbo Etiqueta</a> / <a href="http://www.habbo.com.br/safety/safety_tips">Segurança</a></div>
        <div id="copyright">© 2008 - 2015 Sulake Corporation Ltd. HABBO é marca registrada da Sulake Corporation Oy na União Européia, EUA, Japão, China e várias outras jurisdições. Todos os direitos reservados.</div>
    </div>
<div id="sulake-logo"><a href="http://dubbo.biz" style="background-image: url('http://dubbo.biz/images/dmca_protected.png')"></a></div>

</footer>

<script src="<?php echo $path; ?>/habblet/js/v3_landing_bottom2.js" type="text/javascript"></script>
<!--[if IE]><script src="<?php echo $path; ?>/habblet/js/v3_ie_fixes.js" type="text/javascript"></script>
<![endif]-->

</body>
</html>