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
//запрос с получением чата по имени. ≈сли его нет, то создаЄт новый с именем чата и главой чата
$sth = $dbh->prepare('SELECT * FROM myDB.chat WHERE (myDB.chat.name = :pr_name)');
$name=$_GET['php_name'];
$sth->bindParam(':pr_name',$name,PDO::PARAM_STR);
$sth->execute();
$sth->setFetchMode(PDO::FETCH_ASSOC);
$result = $sth->fetchAll(); 
if (count($result) == 0) 
{
	$sth =$dbh->prepare('insert into myDB.chat (myDB.chat.name) values (:pr_name1)');

	$sth->bindParam(':pr_name1',$name,PDO::PARAM_STR);
	$sth->execute();

	$adm='admin';
	$sth =$dbh->prepare('insert into myDB.list_chats (myDB.list_chats.player,myDB.list_chats.chat,myDB.list_chats.status) values (:pr_player,:pr_name1, :pr_status)');

	$sth->bindParam(':pr_name1',$name,PDO::PARAM_STR);
	$sth->bindParam(':pr_player',$_GET['php_player'],PDO::PARAM_STR);
	$sth->bindParam(':pr_status',$adm,PDO::PARAM_STR);
	$sth->execute();
	echo 'true';
}
else{
	echo 'false';
}
?>