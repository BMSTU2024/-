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
 //отправляет запрос на изменение статуса игрока чата на [удалённый]
//$sth = $dbh->query('SELECT * FROM myDB.list_chats where');
$str=$_GET['php_player'];
$str1=$_GET['php_chat'];
$str2 ='deleted';
//$sth = $dbh->prepare('delete from myDB.list_chats where (list_chats.player=:pr_player and myDB.list_chats.chat=:pr_chat)');
$sth = $dbh->prepare('update myDB.list_chats set status=:pr_status where (list_chats.player=:pr_player and myDB.list_chats.chat=:pr_chat)');

	$sth->bindParam(':pr_player',$str,PDO::PARAM_STR);
	$sth->bindParam(':pr_chat',$str1,PDO::PARAM_STR);
	$sth->bindParam(':pr_status',$str2,PDO::PARAM_STR);

$sth->execute();



/*
$sth->setFetchMode(PDO::FETCH_ASSOC);
 
$result = $sth->fetchAll(); 
if (count($result) > 0) 
{
	$sth = $dbh->prepare('update myDB.list_chats set (status=:pr_status) where (list_chats.player!=:pr_player and myDB.list_chats.chat=:pr_chat)');
	$sth->bindParam(':pr_player',$str,PDO::PARAM_STR);
	$sth->bindParam(':pr_chat',$str1,PDO::PARAM_STR);
	$sth->bindParam(':pr_status','waiting',PDO::PARAM_STR);
}
else{
	$sth = $dbh->prepare('insert into myDB.list_chats (player,chat,status) values (:pr_player,:pr_chat,:pr_status) ');
	$sth->bindParam(':pr_player',$str,PDO::PARAM_STR);
	$sth->bindParam(':pr_chat',$str1,PDO::PARAM_STR);
	$sth->bindParam(':pr_status','waiting',PDO::PARAM_STR);
}
*/
?>