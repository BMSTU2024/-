<?php
$hostname = 'localhost';
$username = 'root';
$password = '';
$database = 'myDB';
 
try 
{
	$dbh = new PDO('mysql:host='. $hostname .';dbname='. $database, $username, $password);
} 
catch(PDOException $e) 
{
	echo '<h1>An error has occurred.</h1><pre>', $e->getMessage()
            ,'</pre>';
}
//запрос с изменением статуса конкретного игрока
$sth = $dbh->prepare('update myDB.player set player.status=:pr_status where player.login=:pr_login');
try{
	$sth->bindParam(':pr_status',$_POST['php_status'],PDO::PARAM_STR);
	$sth->bindParam(':pr_login',$_POST['php_login'],PDO::PARAM_STR);
	$sth->execute();
}catch(Exception $e){
}


?>