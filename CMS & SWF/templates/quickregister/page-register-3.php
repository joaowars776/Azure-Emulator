<body id="client" class="background-captcha">
<div id="overlay"></div>
<img src="<?php echo $path; ?>/web-gallery/v2/images/page_loader.gif" style="position:absolute; margin: -1500px;" />

<div id="change-password-form" style="display: none;">

    <div id="change-password-form-container" class="clearfix">
        <div id="change-password-form-title" class="bottom-border">Esqueceu sua senha?</div>
        <div id="change-password-form-content" style="display: none;">
            <form method="post" action="https://www.habbo.es/account/password/identityResetForm" id="forgotten-pw-form">
                <input type="hidden" name="page" value="/quickregister/captcha?changePwd=true" />
                <span>Digite seu e-mail <?php echo $shortname; ?> conta:</span>
                <div id="email" class="center bottom-border">
                    <input type="text" id="change-password-email-address" name="emailAddress" value="" class="email-address" maxlength="48"/>

                    <div id="change-password-error-container" class="error" style="display: none;">Por favor, indique um e-mail</div>
                </div>
            </form>
            <div class="change-password-buttons">
                <a href="#" id="change-password-cancel-link">Cancelar</a>
                <a href="#" id="change-password-submit-button" class="new-button"><b>Enviar email</b><i></i></a>
            </div>

        </div>
        <div id="change-password-email-sent-notice" style="display: none;">
            <div class="bottom-border">
                <span>Enviamos um e-mail para o seu endereço de e-mail com o link que você precisa clicar para alterar sua senha.</span>
                <div id="email-sent-container"></div>
            </div>
            <div class="change-password-buttons">
                <a href="#" id="change-password-change-link">Volta</a>

                <a href="#" id="change-password-success-button" class="new-button"><b>Fechar</b><i></i></a>
            </div>
        </div>
    </div>
    <div id="change-password-form-container-bottom"></div>
</div>

<script type="text/javascript">
HabboView.add( function() {
     ChangePassword.init();


});
</script>

<div id="stepnumbers">
    <div class="stepdone">Aniversário e Sexo</div>
    <div class="stepdone">Detalhes da conta</div>
    <div class="step3focus">Verificações de segurança</div>
    <div class="stephabbo"></div>
</div>

<div id="main-container"> 
<?php if(isset($errors)){ ?>

<div id="error-messages-container" class="cbb"> 
          <div class="rounded" style="background-color: #cb2121;"> 
          <div id="error-title" class="error"><?php echo $errors; ?>
	  </div></div></div>
<?php } ?>

    <h2>Muda-se para o Hotel</h2>


   <div id="bubble-container" class="cbb">
        <div id="bubble-content" class="rounded">
             <div id="bubble-title">
                Verificação de Segurança
            </div>
            <div id="captcha-image-container">
                <div id="recaptcha_image"><img src="<?php echo $path; ?>/captcha/captcha.php"></div>
            </div>
            <div id="captcha-reload-container">

                <a id="recaptcha-reload" href="#">Tente palavras diferentes</a>
            </div>    
        </div>
    </div>

    <div class="delimiter_smooth">
        <div class="flat">&nbsp;</div>
        <div class="arrow">&nbsp;</div>
        <div class="flat">&nbsp;</div>

    </div>

    <div id="inner-container">
        <form id="captcha-form" method="post" action="<?php echo $path; ?>/quickregister/captcha_submit" onSubmit="Overlay.show(null,'Loading...');">
            <div id="recaptcha-input-title">Escrever cartas (sem separá-los por espaços):</div>
            <div id="recaptcha-input">
                <input type="text" tabindex="2" name="captchaResponse" id="recaptcha_response_field">
            </div>
        </form>

    </div>

    <div id="select">
        <a href="<?php echo $path; ?>/quickregister/backToAccountDetails" id="back-link">Voltar</a>
        <div class="button">
            <a id="proceed-button" href="#" class="area">Finalizar</a>
            <span class="close"></span>
        </div>

   </div>

<script type="text/javascript">
    document.observe("dom:loaded", function() {
        Event.observe($("back-link"), "click", function() {
            Overlay.show(null,'Carregamento...');
        });
        Event.observe($("proceed-button"), "click", function() {
            Overlay.show(null,'Carregamento...');            
            $("captcha-form").submit();
        });
            $("captcha-form").focus();
    });
</script>



</div> 
 
<script type="text/javascript"> 
    HabboView.run();
</script> 
 
</body> 
</html>