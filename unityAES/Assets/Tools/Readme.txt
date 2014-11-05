#Readme#



#AES#
aes是一个加密二进制类，要得到不同的加密规则可以修改AES.cs中的Key属性。

#Xml#
1、对固有xml加密，可以在Assets=>Assets/Xml Encryption for New Path.并选择一个新的路径，如果是原路径会覆盖掉
现在有文件。请注意！
2、动态加密解密，请看TestDome.cs里的“打印”与“写入”.

#AssetBundle#
打包资源加密
1、点击Assets=>Open Assetsbundle Windon，选择平台，选择是否是复合回打包（多个文件打成一个包），在选中你要打包的文件。点击“加密创建”，选择文件夹，文件夹的路径只能是Assets中，不能超出。
得到你的加密的assetBundle，和一个同名的xml配置文件。

调用
1、
			//设置模型
		 ABS.debug = false;
		//添加事件 
        ABS.OnLoaded += Loaded;
		//开始加载资源
        ABS.StartLoadRes("test1", this);
		
		//这里的test1是你xml文件名，this是你调用的脚本本身，一定要继承 MonoBehaviour;
		
		
		  Instantiate(ABS.GetRes("Cube"), new Vector3(x, y, z), Quaternion.identity);
		  //调用只要使用ABS.GetRes()方法就可以得到一个GameObject.参数为你原始的资源名。
2、debug变量可以设置是否是开发模式。
		开发模型下直接调用的是Resources里的资源，不是加密资源，只有在发布的时候才用加密资源。
	

#最后#
直接看TestDome