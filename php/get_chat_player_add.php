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
 //������ � ���������� ���� �������, ��� �� ������� � ���������� ���� � ��� ���������� �� ����������� ������
//$sth = $dbh->query('SELECT * FROM myDB.list_chats where');
$str=$_GET['php_player'];
//$sth = $dbh->prepare('SELECT * FROM myDB.player,myDB.list_chats WHERE (myDB.player.login!=myDB.list_chats.player and myDB.list_chats.chat=:pr_chat and myDB.player.login like :pr_player) ');



$sth = $dbh->prepare('SELECT * FROM myDB.player where( ((select count(*) from myDB.list_chats where(myDB.list_chats.chat=:pr_chat and myDB.list_chats.player=myDB.player.login) )=0 or

(select count(*) from myDB.list_chats where(myDB.list_chats.chat=:pr_chat1 and myDB.list_chats.player=myDB.player.login and myDB.list_chats.status= :pr_status) )=1) and myDB.player.login like :pr_player

) ');
$strr=$str.'%';
$sth->bindParam(':pr_player',$strr,PDO::PARAM_STR);
$pr_st='deleted';
$pr_st1=$_GET['php_chat'];
$sth->bindParam(':pr_status',$pr_st,PDO::PARAM_STR);
$sth->bindParam(':pr_chat',$pr_st1,PDO::PARAM_STR);
$sth->bindParam(':pr_chat1',$pr_st1,PDO::PARAM_STR);
$sth->execute();
$sth->setFetchMode(PDO::FETCH_ASSOC);
 
$result = $sth->fetchAll(); 
if (count($result) > 0) 
{
	foreach($result as $r) 
	{
		echo $r['login'],"_";
		//echo $r['password'], "_";
	}
}
?>