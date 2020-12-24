[TOC]

# database-project

Big project for course design: Database.



## 接口文档说明

### 1，基地址

以下的接口都带有一个基地址，设置为/api，也就是说，如果你想访问/login的接口，就必须要访问/api/login才能访问成功。

### 2，编码

传输全部使用UTF-8编码，理论上讲，只要不要乱指定字符集，默认的就是UTF-8。但是要注意字符的转义（应该有库函数会自动实现的，似乎flyio是会自动帮你做这一步的），比如说空格要变成%20等等。

### 3，需要使用到的对象

需要用到的对象我会在后面用专门的章节写出来，如果我在接口中说这里要提供一个XXX对象，请根据目录直接进行查找。

### 4，请求方法

我们仅限定于POST，GET和DELETE方法。请注意这三种方法内涵上的差异，一般而言，给服务器提交一个数据，使用POST方法，从服务器获取一个数据，使用GET方法，让服务器删除一个东西，使用DELETE方法。

### 5，接口传输的格式

接口采用三种形式进行约定，一种为request parameter，另一种为path variable，最后一种是request body。这三种参数所处的位置在不同的地方，示例如下：

#### 1，request parameter

这种是最常见的，也就是拿？和&进行拼接的那种，比如说：

```
http://localhost:8080/login?userName=czf&password=123456
```

userName和password就是请求的参数。这样的参数可以提供，也可以不提供，具体看我的接口的约定。这样的参数一般只可以拿来传简单的数据，比如说int，double，string或者array等等，不能传递复杂的数据。

#### 2，path variable

例如在一些有特定含义或者不会引起歧义的情况下，上面的那种接口显得有点臃肿和复杂，我们可以使用另外一种restful风格的url，举个例子，在百度百科上搜索某个关键字的时候，它的地址栏就是这样的：

![image-20201222171941912](https://pic-bed.xyz/res/userFiles/czf/122.jpg)

你会发现我搜索的“华工”关键词在地址栏以路径的形式被传递进去了。这种叫做path variable。同样的道理，path variable不能传输过于复杂的数据，它对于关键字更加敏感，你必须保证输入的内容里不会有斜杠，加号，问号之类的特殊字符（如果实在有的话，必须要进行转义），否则会出错。

#### 3，request body

前面说的两种不能传递复杂数据的方式，接下来就给一种可以传递复杂数据的格式，就是从请求体来。请求体理论上来讲是不会限制文件大小的，也不会限制文件格式，至少不会限制是int，double还是啥。所以说，如果需要提交一个json，或者上传一个文件，请把它放进去请求体里而不是在url里拼接参数。

### 6，缺省的异常处理

任何一个接口都有可能出错，在接口文档里我写出了一些可能会预料到的错误，并且会给出相应的提示。如果发生了意料之外的错误，会返回500状态码。



## 接口文档

### 1，学生用户注册

路径：/signUp

方法：POST

参数：

| 名称     | 数据类型        | 存在位置          | 是否必须提供 | 描述           |
| -------- | --------------- | ----------------- | ------------ | -------------- |
| userName | 字符串          | request parameter | 是           | 用户名         |
| password | 字符串          | request parameter | 是           | 密码           |
| (json)   | Student类的对象 | request body      | 是           | 学生的详细信息 |

返回值：

对应生成的Student类的对象。

异常处理：

| 状态码 | 解释                               |
| ------ | ---------------------------------- |
| 400    | 用户名已经被使用过了（后端会提示） |
| 400    | 手机号已经被使用过了（后端会提示） |
| 400    | 学号已经被使用过了（后端会提示）   |

补充：虽然在Student类里已经定义了userName这个字段，但是为了保险起见，还是在请求体里再传一次会比较好，所以务必要保证两个地方的userName是一致的。



### 2，管理员注册

路径：/adminSignUp

方法：POST

参数：

| 名称     | 数据类型      | 存在位置          | 是否必须提供 | 描述             |
| -------- | ------------- | ----------------- | ------------ | ---------------- |
| userName | 字符串        | request parameter | 是           | 用户名           |
| password | 字符串        | request parameter | 是           | 密码             |
| (json)   | Admin类的对象 | request body      | 是           | 管理员的详细信息 |

返回值：

对应生成的Admin类的对象。

异常处理：

| 状态码 | 解释                               |
| ------ | ---------------------------------- |
| 400    | 用户名已经被使用过了（后端会提示） |
| 400    | 手机号已经被使用过了（后端会提示） |
| 400    | 工号已经被使用过了（后端会提示）   |



### 3，登录（学生和管理员登录都在这个接口）

路径：/login

方法：POST

参数：

| 名称     | 数据类型           | 存在位置          | 是否必须提供 | 描述   |
| -------- | ------------------ | ----------------- | ------------ | ------ |
| userName | 字符串             | request parameter | 是           | 用户名 |
| password | 字符串             | request parameter | 是           | 密码   |
| role     | 整数（角色枚举类） | request parameter | 是           | 角色   |

返回值：

对应生成的Credential类的对象。

异常处理：

| 状态码 | 解释             |
| ------ | ---------------- |
| 400    | 太多设备同时登陆 |
| 401    | 用户名或密码错误 |



### 4，退出登录

路径：/logOut

方法：POST

参数：

| 名称  | 数据类型 | 存在位置          | 是否必须提供 | 描述     |
| ----- | -------- | ----------------- | ------------ | -------- |
| token | 字符串   | request parameter | 是           | 用户令牌 |

返回值：

无

异常处理：

无



### 5，强制退出登录（危险）

路径：/logOutAnyway

方法：POST

参数：

| 名称     | 数据类型 | 存在位置          | 是否必须提供 | 描述   |
| -------- | -------- | ----------------- | ------------ | ------ |
| userName | 字符串   | request parameter | 是           | 用户名 |

返回值：

无

异常处理：

无



### 6，修改学生个人信息

路径：/self

方法：POST

参数：

| 名称   | 数据类型      | 存在位置     | 是否必须提供 | 描述     |
| ------ | ------------- | ------------ | ------------ | -------- |
| (json) | Student类对象 | request body | 是           | 学生信息 |

返回值：

无

异常处理：

无



### 7，修改管理员个人信息

路径：/adminSelf

方法：POST

参数：

| 名称   | 数据类型    | 存在位置     | 是否必须提供 | 描述       |
| ------ | ----------- | ------------ | ------------ | ---------- |
| (json) | Admin类对象 | request body | 是           | 管理员信息 |

返回值：

无

异常处理：

无



### 8，修改密码（学生和管理员修改密码都是这个）

路径：/changePassword

方法：POST

参数：

| 名称        | 数据类型 | 存在位置          | 是否必须提供 | 描述   |
| ----------- | -------- | ----------------- | ------------ | ------ |
| userName    | 字符串   | request parameter | 是           | 用户名 |
| oldPassword | 字符串   | request parameter | 是           | 旧密码 |
| newPassword | 字符串   | request parameter | 是           | 新密码 |

返回值：

无

异常处理：

| 状态码 | 解释                               |
| ------ | ---------------------------------- |
| 400    | 查无此人（后端会提示）             |
| 400    | 用户名与原密码不匹配（后端会提示） |



### 9，发布商品

路径：/commodity/new

方法：POST

参数：

| 名称     | 数据类型        | 存在位置          | 是否必须提供 | 描述     |
| -------- | --------------- | ----------------- | ------------ | -------- |
| userName | 字符串          | request parameter | 是           | 用户名   |
| (json)   | Commodity类对象 | request body      | 是           | 商品信息 |

返回值：

对应的Commodity类对象

异常处理：

无



### 10，修改商品信息

路径：/commodity/update

方法：POST

参数：

| 名称   | 数据类型        | 存在位置     | 是否必须提供 | 描述     |
| ------ | --------------- | ------------ | ------------ | -------- |
| (json) | Commodity类对象 | request body | 是           | 商品信息 |

返回值：

对应的Commodity类对象

异常处理：

无



### 11，删除商品

路径：/commodity/delete

方法：DELETE

参数：

| 名称        | 数据类型 | 存在位置          | 是否必须提供 | 描述   |
| ----------- | -------- | ----------------- | ------------ | ------ |
| commodityId | 整数     | request parameter | 是           | 商品id |

返回值：

对应的Commodity类对象

异常处理：

| 状态码 | 解释             |
| ------ | ---------------- |
| 400    | 该商品已经被售出 |



### 12，搜索商品

路径：/commodity/search

方法：GET

参数：

| 名称  | 数据类型 | 存在位置          | 是否必须提供 | 描述         |
| ----- | -------- | ----------------- | ------------ | ------------ |
| query | 字符串   | request parameter | 是           | 搜索的关键字 |

返回值：

Commodity类对象组成的List，因为搜索结果可能有多个。

异常处理：

| 状态码 | 解释                           |
| ------ | ------------------------------ |
| 400    | 输入的关键词为空或者有非法字符 |

如果没有匹配的搜索结果，返回的是空列表，不会报错



### 13，查找确定的一个商品

路径：/commodity/{commodityId}

注意这个是路径参数，请求时不要写commodityId=xxx

方法：GET

参数：

| 名称        | 数据类型 | 存在位置      | 是否必须提供 | 描述   |
| ----------- | -------- | ------------- | ------------ | ------ |
| commodityId | 整数     | path variable | 是           | 商品id |

返回值：

对应的Commodity类对象

异常处理：

无



### 14，购买商品

路径：/sales/buy

方法：POST

参数：

| 名称        | 数据类型            | 存在位置          | 是否必须提供 | 描述         |
| ----------- | ------------------- | ----------------- | ------------ | ------------ |
| userName    | 字符串              | request parameter | 是           | 用户名       |
| commodityId | 整数                | request parameter | 是           | 商品id       |
| (json)      | AddressDetail类对象 | request body      | 是           | 收货地址详情 |

返回值：

对应SalesRecord类对象

异常处理：

| 状态码 | 解释     |
| ------ | -------- |
| 400    | 竞争购买 |



### 15，确认收货

路径：/sales/confirm

方法：POST

参数：

| 名称        | 数据类型 | 存在位置          | 是否必须提供 | 描述         |
| ----------- | -------- | ----------------- | ------------ | ------------ |
| salesRecord | 整数     | request parameter | 是           | 销售记录的id |

返回值：

对应SalesRecord类对象

异常处理：

无。服务端会忽略重复签收的错误，不会报错。



### 16，评价此次交易

路径：/sales/comment

方法：POST

参数：

| 名称        | 数据类型 | 存在位置          | 是否必须提供 | 描述         |
| ----------- | -------- | ----------------- | ------------ | ------------ |
| salesRecord | 整数     | request parameter | 是           | 销售记录的id |
| comment     | 字符串   | request parameter | 是           | 评价         |

返回值：

对应SalesRecord类对象

异常处理：

| 状态码 | 解释               |
| ------ | ------------------ |
| 400    | 没有签收不能评价。 |

如果重复提交的话，会覆盖之前的评价记录，不会报错。



### 17，查看自己的交易、发布记录

路径：/statistic/myCommodities

方法：GET

参数：

| 名称     | 数据类型 | 存在位置          | 是否必须提供 | 描述   |
| -------- | -------- | ----------------- | ------------ | ------ |
| userName | 字符串   | request parameter | 是           | 用户名 |

返回值：

对应MyGoods类对象

异常处理：

无。如果没有相应记录的话，返回的是空列表，不会报错。



### 18，列出所有学生信息

路径：/statistic/listStudents

方法：GET

参数：

| 名称  | 数据类型 | 存在位置          | 是否必须提供 | 描述                                          |
| ----- | -------- | ----------------- | ------------ | --------------------------------------------- |
| limit | 整数     | request parameter | 否           | 限制返回的个数，如果不提供此参数，默认为100个 |

返回值：

对应Student类对象组成的List

异常处理：

无。如果没有相应记录的话，返回的是空列表，不会报错。



### 19，列出所有管理员信息

路径：/statistic/listAdmins

方法：GET

参数：

| 名称  | 数据类型 | 存在位置          | 是否必须提供 | 描述                                          |
| ----- | -------- | ----------------- | ------------ | --------------------------------------------- |
| limit | 整数     | request parameter | 否           | 限制返回的个数，如果不提供此参数，默认为100个 |

返回值：

对应Admin类对象组成的List

异常处理：

无。如果没有相应记录的话，返回的是空列表，不会报错。



### 20，列出所有商品信息

路径：/statistic/listCommodities

方法：GET

参数：

| 名称  | 数据类型 | 存在位置          | 是否必须提供 | 描述                                          |
| ----- | -------- | ----------------- | ------------ | --------------------------------------------- |
| limit | 整数     | request parameter | 否           | 限制返回的个数，如果不提供此参数，默认为100个 |

返回值：

对应Commodity类对象组成的List

异常处理：

无。如果没有相应记录的话，返回的是空列表，不会报错。



### 21，列出所有交易记录

路径：/statistic/listSalesRecords

方法：GET

参数：

| 名称  | 数据类型 | 存在位置          | 是否必须提供 | 描述                                          |
| ----- | -------- | ----------------- | ------------ | --------------------------------------------- |
| limit | 整数     | request parameter | 否           | 限制返回的个数，如果不提供此参数，默认为100个 |

返回值：

对应SalesRecord类对象组成的List

异常处理：

无。如果没有相应记录的话，返回的是空列表，不会报错。



### 22，管理员修改学生信息

路径：/statistic/modifyStudent

方法：POST

参数：

| 名称   | 数据类型      | 存在位置     | 是否必须提供 | 描述     |
| ------ | ------------- | ------------ | ------------ | -------- |
| (json) | Student类对象 | request body | 是           | 学生信息 |

返回值：

对应Student类对象

异常处理：

无。但是，不可以修改学生用户名！



### 23，删除学生

路径：/statistic/deleteStudent

方法：DELETE

参数：

| 名称  | 数据类型 | 存在位置          | 是否必须提供 | 描述   |
| ----- | -------- | ----------------- | ------------ | ------ |
| stuId | 整数     | request parameter | 是           | 学生id |

返回值：

对应的Student类对象

异常处理：

| 状态码 | 解释                                                         |
| ------ | ------------------------------------------------------------ |
| 400    | 这个学生已经发布过商品或者购买过商品，直接删除学生会引起数据丢失，删除请求被拒绝。 |



### 24，删除管理员

路径：/statistic/deleteAdmin

方法：DELETE

参数：

| 名称      | 数据类型 | 存在位置          | 是否必须提供 | 描述           |
| --------- | -------- | ----------------- | ------------ | -------------- |
| teacherId | 整数     | request parameter | 是           | 管理员的用户id |

返回值：

对应的Admin类对象

异常处理：

无。除非删了一个本不存在的数据。



### 25，删除商品

路径：/statistic/deleteCommodity

方法：DELETE

参数：

| 名称        | 数据类型 | 存在位置          | 是否必须提供 | 描述   |
| ----------- | -------- | ----------------- | ------------ | ------ |
| commodityId | 整数     | request parameter | 是           | 商品id |

返回值：

对应的Commodity类对象

异常处理：

无。除非删了一个本不存在的数据。

注意，无论是删除商品还是删除交易记录，都会引起级联的商品和交易记录删除。也就是说，这个商品和交易记录都会跟着被删除。这是一个相当危险的操作，极其不建议进行此操作。



### 26，删除交易记录

路径：/statistic/deleteSalesRecord

方法：DELETE

参数：

| 名称        | 数据类型 | 存在位置          | 是否必须提供 | 描述   |
| ----------- | -------- | ----------------- | ------------ | ------ |
| commodityId | 整数     | request parameter | 是           | 商品id |

返回值：

对应的SalesRecord类对象

异常处理：

无。除非删了一个本不存在的数据。

注意，无论是删除商品还是删除交易记录，都会引起级联的商品和交易记录删除。也就是说，这个商品和交易记录都会跟着被删除。这是一个相当危险的操作，极其不建议进行此操作。



## 需要用到的对象

### 1，Student类

```c#
public class Student {
    public int Id { get; set; } //用户id
	public string UserName { get; set; } = "default"; //用户名。必填
    public string IconURL { get; set; } = "https://pic-bed.xyz/res/icons/default.png"; //头像图片的地址
	public Gender Gender { get; set; } = Gender.SECRET; //性别，是枚举类，具体见Gender的定义
    public string Name { get; set; } //真实姓名，必填
    public string SerialNumber { get; set; } //学号，必填
    public string Profile { get; set; } = "这个人很懒，什么简介都没有写"; //个人简介
    public string Phone { get; set; } //电话号码，必填
    public string Email { get; set; } = ""; //电子邮箱，可不填
   	public DateTimeOffset Birthday { get; set; } //生日，满足ISO格式的标准时间戳
    public int Age { get; } //前端无需提供该值，会根据生日自动算
    
    public string College { get; set; } //学院
	public string Major { get; set; } //专业
	public string Dormitory { get; set; } = ""; //宿舍号
}
```

### 2，性别枚举类

```c#
public enum Gender {
    MALE,
    FEMALE,
    UNKNOWN, //特殊性别
    SECRET //保密
}
```

### 3，Admin类

```c#
public class Admin {
    public int Id { get; set; } //用户id
	public string UserName { get; set; } = "default"; //用户名。必填
    public string IconURL { get; set; } = "https://pic-bed.xyz/res/icons/default.png"; //头像图片的地址
	public Gender Gender { get; set; } = Gender.SECRET; //性别，是枚举类，具体见Gender的定义
    public string Name { get; set; } //真实姓名，必填
    public string SerialNumber { get; set; } //工号，必填
    public string Profile { get; set; } = "这个人很懒，什么简介都没有写"; //个人简介
    public string Phone { get; set; } //电话号码，必填
    public string Email { get; set; } = ""; //电子邮箱，可不填
   	public DateTimeOffset Birthday { get; set; } //生日，满足ISO格式的标准时间戳
    public int Age { get; } //前端无需提供该值，会根据生日自动算
    
	public string Department { get; set; } = "NetWork Center"; //部门
	public int Level { get; set; } = 1; //此值暂时无用，前端可以随意塞一个数在这里即可
}
```

### 4，角色枚举类

```c#
public enum Role {
    STUDENT,
    ADMIN
}
```

### 5，Credential类

```c#
public class IdentityCredential {
    public string Token { get; set; } //令牌，前端必须要保存这个值
    public dynamic Credential { get; set; } //身份信息，如果是学生登陆，这里就变成Student类，如果是管理员登陆，这里就变成Admin类
    public DateTimeOffset ExpireDate { get; set; } = DateTimeOffset.Now.AddDays(15); //默认15天后令牌失去有效期，要重新登陆
    public Platform Platform { get; set; } = Platform.Web; //登陆平台
}

public enum Platform {
    Android,
    IPhone,
    Web
}
```

### 6，Commodity类

```c#
public class Commodity {
    public int Id { get; set; } //商品id
	public string Title { get; set; } //标题，简介。限制50个字
	public string Description { get; set; } //详细的商品描述
	public List<string> Photos { get; set; } = new List<string>(); //图片的URL，不要把图片扔过来
	public DateTimeOffset ReleaseTime { get; set; } = DateTimeOffset.Now; //发布时间
	public decimal Price { get; set; } = 0.0M; //价格，可以为小数
	public Student Seller { get; set; } //发布商品的那个学生，注意如果是发布商品的时候，这个值可以不写
}
```

### 7，AddressDetail类

```c#
//之所以要设置这个类，是因为用户的信息和收件人的信息不一定一致
public class AddressDetail {
    public string Name { get; set; } //收件人姓名
    public string Address { get; set; } //收件人地址
    public string Phone { get; set; } //收件人电话
}
```

### 8，SalesRecord类

```c#
public class SalesRecord {
    public int Id { get; set; } //销售记录id
	public int CommodityId { get; set; } //对应的商品id
	public Commodity Commodity { get; set; } //商品实体
	public Student Seller { get; set; } //卖家
	public Student Buyer { get; set; } //买家
	public AddressDetail DeliveryAddress { get; set; } //收货地址
	public string? Comment { get; set; } //交易评价。如果用户没有评价的话，这个字段不会出现在json字符串中
	public DateTimeOffset TransactionTime { get; set; } = DateTimeOffset.Now; //交易时间
	public decimal Auction { get; set; } //成交价，可以写小数
	public bool Check { get; set; } //是否签收？
}
```

### 9，MyGoods类

```c#
public class MyGoods {
    public List<Commodity> AllMyCommodities { get; set; } = new List<Commodity>(); //我发布的
	public List<SalesRecord> Sold { get; set; } = new List<SalesRecord>(); //我卖出去的
	public List<SalesRecord> Bought { get; set; } = new List<SalesRecord>(); //我买到的，过滤一下其中的check字段即可得到已付款但未签收的商品
}
```

