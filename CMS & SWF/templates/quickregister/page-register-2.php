<body id="client" class="background-accountdetails-male">
<div id="overlay"></div>
<img src="<?php echo $path; ?>/web-gallery/v2/images/page_loader.png" style="position:absolute; margin: -1500px;" />


<div id="change-password-form" style="display: none;">

    <div id="change-password-form-container" class="clearfix">

        <div id="change-password-form-title" class="bottom-border">Esqueceu sua senha?</div>

        <div id="change-password-form-content" style="display: none;">

            <form method="post" action="https://www.habbo.es/account/password/identityResetForm" id="forgotten-pw-form">

                <input type="hidden" name="page" value="/quickregister/email_password?changePwd=true" />

                <span>Por favor, indique o e-mail em sua conta <?php echo $shortname; ?>:</span>

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

                <span>Enviamos um e-mail para o seu endereço de e-mail com o link que você precisa clicar para alterar sua senha.<br>
<br>

NOTA: Lembre-se também verificar 'Spam' a pasta</span>

                <div id="email-sent-container"></div>

            </div>

            <div class="change-password-buttons">

                <a href="#" id="change-password-change-link">Voltar</a>

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

    <div class="step2focus">Detalhes da conta</div>

    <div class="step3">Verificações de segurança</div>

    <div class="stephabbo"></div>

</div>



<div id="main-container">


<?php if(isset($errors)){ ?>

<div id="error-messages-container" class="cbb"> 
          <div class="rounded" style="background-color: #cb2121;"> 
          <div id="error-title" class="error"><?php echo $errors; ?>
	  </div></div></div>
<?php } else {?>
<?php } ?>


    <form method="post" action="<?php echo $path; ?>/quickregister/email_password_submit" id="quickregister-form">



        <div id="title">Detalhes da conta</div>

      <div id="inner-container">

        <div class="inner-content bottom-border">
		  <div class="field">

                <label for="user-address"><?php echo $shortname; ?> Nome</label>

                <input type="text" id="email-address" name="bean.name" value="<?php echo FilterText($_POST['bean_name']); ?>" <?php if(isset($errors['name'])) { echo 'class="error"'; } else { } ?>/>

            </div>
                    <div class="help">Este nome é usado para conectar-se ao hotel, sugerimos que você se lembre de colocar um.</div>

            <div class="field">

                <label for="email-address">Email</label>

                <input type="text" id="email-address" name="bean.email" value="<?php echo FilterText($_POST['bean_email']); ?>" <?php if(isset($errors['email'])) { echo 'class="error"'; } else { } ?>/>

            </div>

            <div class="field">

                <label for="email-address2">Reintroduzir e-mail</label>

                <input type="text" id="email-address2" name="bean.retypedEmail" value="<?php echo FilterText($_POST['bean_retypedEmail']); ?>" <?php if(isset($errors['retypedemail'])) { echo 'class="error"'; } else { } ?>/>

            </div>

            <div class="help">Por favor use um email válido. Certifique-se de entrar no final correta (Exemplo: hotmail.es ou yahoo.es hotmail.com ou yahoo.com)</div>

            <div id="password-field" class="field">

                <label for="register-password">Nova senha</label>

                <input type="password" name="bean.password" id="register-password" maxlength="32" value="" <?php if(isset($errors['password'])) { echo 'class="error"'; } else { } ?>/>

            </div>
			 <div id="password-field" class="field">

                <label for="register-password">Nova senha</label>

                <input type="password" name="bean.retypedPassword" id="register-password" maxlength="32" value="" <?php if(isset($errors['retypedpassword'])) { echo 'class="error"'; } else { } ?>/>

            </div>

            <div class="help">A senha deve ter pelo menos <b>6 caracteres</b> e incluem <b>letras e números.</b></div>

        </div>



        <div class="inner-content top-margin">

			<div class="field-content checkbox <?php if(isset($errors['terms'])) { echo 'error';} else { } ?>">

			  <label>

			    <input type="checkbox" name="bean.termsOfServiceSelection" id="terms" value="true" class="checkbox-field"/>

			    Eu aceito o <a href="<?php echo $path; ?>/disclaimer" target="_blank" onclick="window.open('<?php echo $path; ?>/disclaimer'); return false;">Termos e Condições</a> de <?php echo $shortname; ?>.

			  </label>

			</div>            

			

			

			

        </div>

      </div>

    </form>





    <div id="select">

        <div class="button">

            <a id="proceed-button" href="#" class="area">Continuar</a>

            <span class="close"></span>

        </div>

        <a href="<?php echo $path; ?>/quickregister/back" id="back-link">Voltar</a>

   </div>

</div>



<script type="text/javascript">

    document.observe("dom:loaded", function() {

        Event.observe($("back-link"), "click", function() {

            Overlay.show(null,'Carregamento...');

        });

        Event.observe($("proceed-button"), "click", function() {

            Overlay.show(null,'Carregamento...');            

            $("quickregister-form").submit();

        });

            $("email-address").focus();

    });

</script>

<script type="text/javascript">
    HabboView.run();
</script>

</body>
</html>