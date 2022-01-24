<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>PayPal Integration</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <style>
        #container{
            width: 100%;
            max-width: 500px;
            display: table;
            margin: 150px auto 0;
        }
        .productBlock{
            width: 100%;
            max-width: 200px;
            display: table;
            margin: 0 auto;
            border: 3px solid #666;
            padding: 10px;
        }
    </style>
    <link href="css/bootstrap.min.css" rel="stylesheet" integrity="sha384-giJF6kkoqNQ00vy+HMDP7azOuL0xtbfIcaT9wjKHr8RbDVddVHyTfAAsrekwKmP1" crossorigin="anonymous">

</head>
<body style=" background-image: url('Assets/background.png');">
    <div id="container">
        <div class="productBlock" style="text-align: center;max-width: 300px !important;">
            <p> Compré su licencia de un año de Genius business</p>
            <form action="https://www.paypal.com/cgi-bin/webscr" method="post" target="_top">
                <input type="hidden" name="cmd" value="_s-xclick">
                <input type="hidden" name="hosted_button_id" value="YGER7TDSK5982">
                <table>
                <tr><td><input type="hidden" name="on0" value="Numero de ordenadores con licencia:">Numero de ordenadores con licencia:</td></tr><tr><td><select name="os0">
                    <option value="1 PC">1 PC €97,97 EUR</option>
                    <option value="3 PCs">3 PCs €217,17 EUR</option>
                    <option value="10 PCs">10 PCs €417,17 EUR</option>
                </select> </td></tr>
                <tr><td><input type="hidden" name="on1" value="Email de Licencia:">Email de Licencia:</td></tr><tr><td><input type="text" name="os1" type="email" id="email" data-validation="email" placeholder="Introduce tu correo electronico" maxlength="200"></td></tr>
                <tr><td><div id="aviso" style="background-color:#ffb9c0;color: #9a0000;font-weight: bold;"><p> Por favor ponga el email para la licencia</p></div></td></tr>
                </table>
                <input type="hidden" name="currency_code" value="EUR">
                <input type="image" disabled = "true" id = "buttonpaypal" src="https://www.paypalobjects.com/es_ES/ES/i/btn/btn_buynowCC_LG.gif" border="0" name="submit" alt="PayPal, la forma rápida y segura de pagar en Internet.">
                <img alt="" border="0" src="https://www.paypalobjects.com/es_ES/i/scr/pixel.gif" width="1" height="1">
                </form>
            </div>
            
                
                
    </div>
    <script src="js/jquery-3.5.1.min.js"/>

    <script src="js/bootstrap.bundle.min.js" ></script>
    <script>
    
        $("#email").on('input',function(e){
            
            var caract = new RegExp(/^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/);
            var emailt = $("#email").val();
        
            if (caract.test(emailt)){
                $( "#buttonpaypal" ).prop( "disabled", false );
                $("#aviso").hide();
              
            }
            else{
                $( "#buttonpaypal" ).prop( "disabled", true )
                $("#aviso").show();
                
            }
        });
    
    
    </script>
</body>
</html>