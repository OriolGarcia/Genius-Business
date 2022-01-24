<?php
use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\Exception;
function generateRandomString($length = 10) {
  $characters = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
  $charactersLength = strlen($characters);
  $randomString = '';
  for ($i = 0; $i < $length; $i++) {
      $randomString .= $characters[rand(0, $charactersLength - 1)];
  }
  return $randomString;
}

  
  // The IPN is verified, process it:
  // check whether the payment_status is Completed
  // check that txn_id has not been previously processed
  // check that receiver_email is your Primary PayPal email
  // check that payment_amount/payment_currency are correct
  // process the notification
  // assign posted variables to local variables
  $NumberSTR = "10 PCs";
    $Number=0;
    switch ($NumberSTR) {
      case "1 PC":
          $Number =1;
          break;
      case "3 PCs":
          $Number=3;
          break;
      case "10 PCs":
          $Number=10;
          break;
    }


  $emaillicence =  "ogs1017@hotmail.com";
  if ($emaillicence == "") {
      $emaillicence =  "ogs1017@hotmail.com";
    }
  $serverName = "serveetd.database.windows.net"; // update me
    $connectionOptions = array(
        "Database" => "etddatabase", // update me
        "Uid" => "tothgarsa", // update me
        "PWD" => "504C3dbb9090" // update me
    );

    $conn = sqlsrv_connect($serverName, $connectionOptions);
    //$conn = new PDO("sqlsrv:Server = {$serverName}; Database = etddatabase", "tothgarsa", "504C3dbb9090");
    var_dump($conn);
    $i = 0;
    $licencias =array();
    while($i< $Number){ 
            $claves = array(
              "email" =>$emaillicence ,
              "clave" => generateRandomString(8)
            );
          $tsql= "SELECT * FROM [dbo].[Licencias] WHERE [email] ='".$claves["email"]."' AND (limitdate < GETDATE() AND limitdate is not null) AND id_App = 'GNB2021'" ;
          $getResults= sqlsrv_query($conn, $tsql);
          var_dump($getResults);
            echo $tsql;
          if ($getResults == FALSE){
            $query ="INSERT INTO [dbo].[Licencias]  ([id_App], [email], [program_key])     VALUES ('GNB2021', '{$claves["email"]}', '{$claves["clave"]}')";
            $stmt = sqlsrv_query( $conn, $query);
            var_dump($stmt);
            echo $query;
          }
          while ($row = sqlsrv_fetch_array($getResults, SQLSRV_FETCH_ASSOC)){
            $query="UPDATE program_key = {$claves["clave"]} , mac=NULL Where id = {$row['id']}";
            $stmt = sqlsrv_query( $conn, $query);
            echo $query;
            }
          sqlsrv_free_stmt($getResults);
          array_push($licencias,$claves);
          $i =$i +1;
      }

  $item_name = "itemname";
  $item_number ="item number";
  $payment_status = "payment status";
  $payment_amount = 'mc_gross';
  $payment_currency = 'mc_currency';
  $txn_id = 'txn_id';
  $receiver_email = 'receiver_email';
  $payer_email = 'ogs1017@hotmail.com';
  // IPN message values depend upon the type of notification sent.
  
  $mail = new PHPMailer;
  $mail->setFrom('oriolgarciao@supremagency.com', 'Suprem Agency');
  $mail->addAddress($payer_email, 'My Friend');
  $mail->Subject  = 'Gracias por comprar Genius Business';

  $body ="<p>&nbsp;</p>";
  $body .= "<p>Hola ".$emaillicence.",</p>";
  $body .= "<p>Gracias por comprar nuestro producto de gesit&oacute;n empresarial,</p>";
  $body .= "<p>Esperamos que el producto le sea de gran utilidad.</p>";
  $body .= "<p>&nbsp;</p>";
  $body .= "<p>Estas son sus licencias:</p>";
  $body .= "<p>&nbsp;</p>.";
  foreach ($licencias as $licencia) {
    

  $body .= "<br><p style=\"padding-left: 90px;\">Email: {$emaillicence}</p>";
  $body .= "<p style=\"padding-left: 90px;\">Clave:{$licencia['clave']}}</p>";

  $body .= "<p style=\"padding-left: 90px;\">&nbsp;</p>";
  }
  $body .= $body . "<p>&nbsp;Si tiene algun problema no dude en contactarnos.</p>";
  $body .= "<p>&nbsp;</p>";
  $body .= "<p>Saludos cordiales.</p>";



    $mail->Body     = $body;
    //$mail->addAttachment('attachment/wpplugin.zip');
    $mail->send();



?>