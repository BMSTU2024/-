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
$str=$_GET['player'];
$pr='deleted';
//запрос с получением всех игроков конкретного чата(кроме удалённых)
if($str==null || true)
	$sth = $dbh->prepare('SELECT myDB.list_chats.chat,myDB.list_chats.player,myDB.list_chats.id,myDB.list_chats.status as st1,myDB.player.status as st2 FROM myDB.list_chats inner join myDB.player on (player=login) WHERE( myDB.list_chats.chat=:pr_chat and myDB.list_chats.status!=:pr_status)');

/*else {
	$sth = $dbh->prepare('SELECT myDB.list_chats.chat,myDB.list_chats.player,myDB.list_chats.id,myDB.list_chats.status as st1,myDB.player.status as st2 FROM myDB.list_chats inner join myDB.player on (player=login) WHERE myDB.list_chats.chat=:pr_chat and myDB.list_chats.player like :pr_find');
	$strr=$str.'%';
	$sth->bindParam(':pr_find',$strr,PDO::PARAM_STR);
}*/
$sth->bindParam(':pr_status',$pr,PDO::PARAM_STR);
$sth->bindParam(':pr_chat',$_GET['php_chat'],PDO::PARAM_STR);

$sth->execute();
$sth->setFetchMode(PDO::FETCH_ASSOC);
$result = $sth->fetchAll(); 
if (count($result) > 0) 
{
	foreach($result as $r) 
	{
		echo $r['chat'],"_";
		echo $r['player'],"_";
		echo $r['id'],"_";
		echo $r['st1'],"_";
		echo $r['st2'],"_";
	}
}
?>