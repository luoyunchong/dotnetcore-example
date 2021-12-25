# nacos-csharp-demo

### nacos

nacos ��һ��������ԭ��Ӧ�õĶ�̬�����֡����ù���ͷ������ƽ̨����

���������ͨ�� csharp���nacosʵ�ֶ����ǵ����ý��й���

- nacos github https://github.com/alibaba/nacos
- csharp sdk github https://github.com/nacos-group/nacos-sdk-csharp
- csharp sdk �ĵ���https://nacos-sdk-csharp.readthedocs.io/en/latest/introduction/gettingstarted.html
- https://nacos.io/zh-cn/

��װ��ο���https://nacos.io/zh-cn/docs/quick-start.html

### ��ʼ

- windows ����nacos��binĿ¼
```
startup.cmd -m standalone
```

Ĭ��������8848�˿�
- http://localhost:8848/nacos/#/login
- nacos
- nacos

## ����
��¼�󣬴�**�����ռ�**->�½������ռ�->
- ` �����ռ�ID`:�����`cs-test`��ע���·���������Namespace����д��ֵ��
- `�����ռ�����`���ֻ������չʾ���֣���`cs-test`������ֱ�Ӻ������ռ�id��ͬ���ɡ�
- `������`:��������


## Nacos+Console
�½�һ������̨��Ŀ

�����
```
<PackageReference Include="Microsoft.Extensions.Hosting" Version="6.0.0" />
<PackageReference Include="nacos-sdk-csharp" Version="1.2.2" />
```

```csharp
static IHost AppStartup()
{
    var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(context, services);
                    services.AddTransient<App>();
                })
                .ConfigureAppConfiguration((host, config) =>
                {

                })
                .Build(); // Build the Host

    return host;
}

```

����nacos�ķ���
```csharp
static void ConfigureServices(HostBuilderContext context,IServiceCollection services)
{
    services.AddNacosV2Config(x =>
    {
        x.ServerAddresses = new System.Collections.Generic.List<string> { "http://localhost:8848/" };
        x.EndPoint = "";
        x.Namespace = "cs-test";

        /*x.UserName = "nacos";
       x.Password = "nacos";*/

        // swich to use http or rpc
        x.ConfigUseRpc = true;
    });

    services.AddNacosV2Naming(x =>
    {
        x.ServerAddresses = new System.Collections.Generic.List<string> { "http://localhost:8848/" };
        x.EndPoint = "";
        x.Namespace = "cs-test";

        /*x.UserName = "nacos";
       x.Password = "nacos";*/

        // swich to use http or rpc
        x.NamingUseRpc = true;
    });
}
```

���� 
```csharp
var host = AppStartup();
var service = ActivatorUtilities.CreateInstance<App>(host.Services);
await service.RunAsync(args);
```
App.cs�ļ�����

```csharp
public class App
{
    private readonly ILogger<App> _logger;
    private readonly INacosConfigService _ns;
    public App(ILogger<App> logger, INacosConfigService ns)
    {
        _logger = logger;
        _ns = ns;
    }

    public async Task RunAsync(string[] args)
    {
        await PublishConfig(_ns);
        await GetConfig(_ns);
        await RemoveConfig(_ns);
    }

    static async Task PublishConfig(INacosConfigService svc)
    {
        var dataId = "demo-dateid";
        var group = "demo-group";
        var val = "test-value-" + DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

        await Task.Delay(500);
        var flag = await svc.PublishConfig(dataId, group, val);
        Console.WriteLine($"======================�������ý����{flag}");
    }

    static async Task GetConfig(INacosConfigService svc)
    {
        var dataId = "demo-dateid";
        var group = "demo-group";

        await Task.Delay(500);
        var config = await svc.GetConfig(dataId, group, 5000L);
        Console.WriteLine($"======================��ȡ���ý����{config}");
    }

    static async Task RemoveConfig(INacosConfigService svc)
    {
        var dataId = "demo-dateid";
        var group = "demo-group";

        await Task.Delay(500);
        var flag = await svc.RemoveConfig(dataId, group);
        Console.WriteLine($"=====================ɾ�����ý����{flag}");
    }
}

```

f5���к�ɿ�������������� 

```
======================�������ý����True
======================��ȡ���ý����test-value-1637000754
=====================ɾ�����ý����True
```

���ǰ�`await RemoveConfig(_ns);`����ɾ����������nacos����վ�Ͽ�����Ϣ��

���ù��� -ѡ`cs-test`,���Կ���`Data IdΪdemo-dateid`��`Group`Ϊ`demo-group`��һ�����ݣ�������ڵı༭���ɿ���������Ϣ��

## ����
- [��һ�������.NET Core��ʹ��Nacos 2.0](https://mp.weixin.qq.com/s/iC6lFJJsHUFUveSJhoZxgA)
