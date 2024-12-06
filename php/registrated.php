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
//запрос с получением игрока по имени. Если такого нет, то добавляет в БД
$sth = $dbh->prepare('SELECT * FROM myDB.player WHERE (myDB.player.login = :pr_login)');

$sth->bindParam(':pr_login',$_POST['php_login'],PDO::PARAM_STR);
$sth->execute();
$sth->setFetchMode(PDO::FETCH_ASSOC);
$result = $sth->fetchAll(); 
if (count($result) == 0) 
{
	$sth = $dbh->prepare('insert into myDB.player(player.login,player.password,player.status) values (:pr_login,:pr_password,:pr_status)');
try{
	$sth->bindParam(':pr_login',$_POST['php_login'],PDO::PARAM_STR);
	$sth->bindParam(':pr_password',$_POST['php_password'],PDO::PARAM_INT);
	$sth->bindParam(':pr_status',$_POST['php_status'],PDO::PARAM_STR);
	$sth->execute();
	echo "good";
}catch(Exception $e){
}
}
else{
	echo "error";
}




?>