<?php
use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\Exception;
require 'vendor/autoload.php';
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
   var_dump($conn); 
   if( $conn ) {
    echo "Conexión establecida.<br />";
  }
  else{
    echo "Conexión no se pudo establecer.<br />";
    die( print_r( sqlsrv_errors(), true));
  }
  

$fecha_actual = date("d-m-Y");
//sumo 1 año
$valida = date("d-m-Y",strtotime($fecha_actual."+ 1 year"));
    $i = 0;
    $licencias =array();
    while($i< $Number){ 
      $claves = array(
        "email" =>$emaillicence ,
        "clave" => generateRandomString(8)
      );
    $tsql= "SELECT * FROM [dbo].[Licencias] WHERE [email] = ? AND (limitdate < GETDATE() AND limitdate is not null) AND id_App = 'GNB2021'" ;
   
   
   $params=array($claves["email"]);
    $getResults= sqlsrv_query($conn, $tsql, $params);

      $row_count = sqlsrv_num_rows( $getResults );
    if ($row_count == 0){
      $query ="INSERT INTO [dbo].[Licencias]  ([id_App], [email], [program_key],[limitdate])     VALUES ('GNB2021', ?, ?, DATEADD(YEAR, 1, GETDATE()))";
      $params=array($claves["email"],$claves["clave"]);
      $stmt = sqlsrv_query( $conn, $query,$params);
      $i =$i +1;
    }
    while ($row = sqlsrv_fetch_array($getResults, SQLSRV_FETCH_ASSOC)){
      $query="UPDATE [dbo].[Licencias] SET program_key = ? , mac=NULL, limitdate =DATEADD(YEAR, 1, GETDATE()) Where id = ?";
     $params=array($claves["clave"],$row['id']);
      $stmt = sqlsrv_query( $conn, $query,$params);
      $i =$i +1;
      }
    sqlsrv_free_stmt($getResults);
    array_push($licencias,$claves);
   
}

      
      
try {

  $item_name = "itemname";
  $item_number ="item number";
  $payment_status = "payment status";
  $payment_amount = 'mc_gross';
  $payment_currency = 'mc_currency';
  $txn_id = 'txn_id';
  $receiver_email = 'receiver_email';
  $payer_email = 'ogs1017@gmail.com';
  // IPN message values depend upon the type of notification sent.

  $mail = new PHPMailer();
  $mail->isSMTP();
  $mail->SMTPDebug = 2;
  $mail->Host = 'mail.privateemail.com';

  $mail->setFrom('ogs1017@gmail.com', 'Suprem Agency');
  $mail->addAddress($payer_email, 'Client');
  $mail->isHTML(true);
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
    $body .= "<p style=\"padding-left: 90px;\">Clave:{$licencia['clave']}</p>";
    $body .= "<p style=\"padding-left: 90px;\">Licencia válida hasta:{$valida}</p>";
    $body .= "<p style=\"padding-left: 90px;\">&nbsp;</p>";
  }
  $body .=  "<p>&nbsp;Si tiene algun problema no dude en contactarnos.</p>";
  $body .= "<p>&nbsp;</p>";
  $body .= "<p>Saludos cordiales.</p>";


  $cabeceras = 'MIME-Version: 1.0' . "\r\n";
  $cabeceras .= 'Content-type: text/html; charset=utf-8' . "\r\n";
  // Cabeceras adicionales

  $cabeceras .= 'From: Genius Business <oriolgarcia@supremagency.com>' . "\r\n";
  mail($payer_email, 'Gracias por comprar Genius Business', $body, $cabeceras);



  /*  echo "<br/> <br/> email 5";
  $mail->SMTPOptions = array(
    'ssl' => array(
        'verify_peer' => false,
        'verify_peer_name' => false,
        'allow_self_signed' => true
    )
);

$mail->Port = 587;
$mail->SMTPAuth = true;
$mail->Username = 'test@hostinger-tutorials.com';
$mail->Password = 'YOUR PASSWORD HERE';

if (!$mail->send()) {
    echo 'Mailer Error: ' . $mail->ErrorInfo;
} else {
    echo 'The email message was sent.';
}
  //$mail->AltBody     = $body;
    //$mail->addAttachment('attachment/wpplugin.zip');
  try {
      if(!$mail->Send()) {
        $error = 'Mail error: '.$mail->ErrorInfo; 
        
    } else {
        $error = 'Message sent!';
        
    }
  } catch (Exception $e) {
      echo "Mailer Error: " . $mail->ErrorInfo;
  } 
*/

    //var_dump($mail);
} catch (phpmailerException $e) {
    echo $e->errorMessage(); //Pretty error messages from PHPMailer
} catch (Exception $e) {
    echo $e->getMessage(); //Boring error messages from anything else!
  }
?>