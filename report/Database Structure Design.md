[TOC]

# Database Structure Design

In this part, we are going to introduce the database we made to support our project.

## Preparation

To make our app as lite as possible, we didn't use a powerful relational database like SQL Server or MySQL. I decided to use SQLite, which is known as a portable database in mobile phones and special devices. This database is very very simple and it only supports String, Numbers, Blobs in database, without stuff like datetime, json, longInt, decimal and so on. But this database has its advantages. First, it is very lite and clean, you don't have to make up a discrete database client for it, and also you don't have to connect the database via TCP connections, which means, the limitation of this database is completely the speed of disks instead of network!

So, the IO efficiency of SQLite is quite horrible, which is several times higher than SQL Server and MySQL, that's because the advantage of direct file access.

But there is also a price, since the SQLite is too lite and too simple, it has some fatal disadvantages which leads to its failure in commercial applications: 1, poor support of data types like time, decimal and so on, so you have to do a lot of data conversion in your server application, that leads to poor performance when dealing with those data. 2, all the data is based on a single file, so the writing and reading process could lead to serious concurrent racing condition, which makes the database poor in high concurrency condition. 3, since it is too simple, it has no support of asynchronous IO, it will cause the long IO waiting and limit the data rate in server.

Anyway, I just want to have a try to apply such a tiny database to a big project to see how it works.



## Analyze

We have four main entities to support the project, they are Student, Administrator, Commodity and SalesRecord.

As we know, Student and Administrator have some attributes in common, such as user name, password, id and so on. So, we should modify them into a hierarchy like: User is the base class, and the Student and Administrator inherit from User. 

Commodity is associated with Student since it must be published by a certain student. By the way, A commodity is also associated with a sales record once a commodity is sold.

SalesRecord is the most diificult one since it connects with the Student by its Buyer and Seller attribute, also, a sales record is binded with a commodity, so this entity has a lot of foreign keys and references. That's what challenge our program.

Besides, we have access control of users which ensures our safety and avoid too many devices of one user. That's another class called LoginRecord.



According to the request, we make up entities as follows, I will introduce what does each attribute mean.

### 1, User Entity

![image-20201226031846828](https://pic-bed.xyz/res/userFiles/czf/142.png)

because of the inheritance between User, Student, Administrator, we end up putting all the attributes in one tale called User, and controls the inheritance by the attibute `Discriminator`, in this way, we successfully handled our inheritance in database.



### 2, Commodity Entity

![image-20201226032350135](https://pic-bed.xyz/res/userFiles/czf/143.png)

We didn't store photos in database directly, but photo URL in database. How to convert a photo to a photo URL? See an open source project developed by 陈治帆 in Github, view the website here: https://github.com/czf0613/picBed



### 3, SalesRecord Entity

![image-20201226034121673](https://pic-bed.xyz/res/userFiles/czf/141.png)

SalesRecord is related to seller, buyer and a commodity, so it has a lot of foreign keys. I will explain what kind of normal form we have adopted.



## ER diagram

According to the restrictions, we created ER diagram like this:



![image-20201226034640164](https://pic-bed.xyz/res/userFiles/czf/144.png)

From the diagram we can see that User(including Students and Administrator) has one-to-many mapping with Commodity since one student can buy or sell several commodities. So does the one-to-many mapping between User and SalesRecord.

By the way, LoginRecord controlls user access and its login status, because we use stateless technology to develop the whole applocation.



## BCNF? ? ?

Actually ,this database design satisfy BCNF except one relation, that is the red link in the graph below:

![image-20201226040017214](https://pic-bed.xyz/res/userFiles/czf/145.png)

This error is made by me on purpose, because the sellerId can be derived by the related commodity, so it is unnecessary and against BCNF. But why we insist on making this mistake? We need to make a balance between the performance and space. Since query of somebody's buying and selling is in high frequency, we must enhance the query efficiency.

If we don't set up the sellerId, we need to find out the commodities associated with someone, and filter it with the `sold` bit, then we can find out how many things you have sold. It is indirect and cause a lot of redundant queries.

If we have this sellerId, checking this case would be directly and fast. So that's why we set up this attribute while breaking the BCNF. In some cases, we must make balance between performance and space.