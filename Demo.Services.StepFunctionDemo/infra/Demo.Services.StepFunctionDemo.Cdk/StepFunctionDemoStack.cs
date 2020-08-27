using System;
using System.Collections.Generic;
using System.Text;
using Amazon.CDK;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.StepFunctions;
using Amazon.CDK.AWS.StepFunctions.Tasks;
using Demo.Services.Lambda;

namespace Demo.Services.StepFunctionDemo.Cdk
{
    public class StepFunctionDemoStack:Stack
    {
        internal StepFunctionDemoStack(Construct scope, string id, IStackProps props = null): base(scope, id, props)
        {
            Bucket stepFunctionDemoBucket = new Bucket(this,"StepFunctionDemoBucket",new BucketProps
            {
                Encryption = BucketEncryption.S3_MANAGED,
                RemovalPolicy = RemovalPolicy.RETAIN
            });

            /*
            //Step Function invoking Lambda function
            Function invokeOddEvenStepFunction = new Function(this, "InvokeOddEvenStepFunction", new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset("src/Demo.Services.Lambda/bin/Release/netcoreapp3.1/Demo.Services.Lambda.zip"),
                Handler = "Demo.Services.Lambda::Demo.Services.Lambda.Functions::InvokeOddEvenStepFunction",
                Timeout = Duration.Minutes(5),
                MemorySize = 512,
                Description = "Lambda Function that invokes the Demo Step Function",

            });
            */

            //Function to calculate Odd or Even
            Function oddOrEvenFunction = new Function(this, "OddOrEvenFunction", new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset("src/Demo.Services.Lambda/bin/Release/netcoreapp3.1/Demo.Services.Lambda.zip"),
                Handler = "Demo.Services.Lambda::Demo.Services.Lambda.Functions::OddOrEvenFunction",
                Timeout = Duration.Minutes(5),
                MemorySize = 512,
                Description = "Lambda Function that calculates odd or even",

            });

            //Demo Lambda to perform Process 1
            Function process1Function = new Function(this, "Process1Function", new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset("src/Demo.Services.Lambda/bin/Release/netcoreapp3.1/Demo.Services.Lambda.zip"),
                Handler = "Demo.Services.Lambda::Demo.Services.Lambda.Functions::Process1Function",
                Timeout = Duration.Minutes(5),
                MemorySize = 512,
                Description = "Demo Lambda Function that runs process1",

            });

            //Demo Lambda to perform Process 2
            Function process2Function = new Function(this, "Process2Function", new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset("src/Demo.Services.Lambda/bin/Release/netcoreapp3.1/Demo.Services.Lambda.zip"),
                Handler = "Demo.Services.Lambda::Demo.Services.Lambda.Functions::Process2Function",
                Timeout = Duration.Minutes(5),
                MemorySize = 512,
                Description = "Demo Lambda Function that runs process2",

            });

            //Demo Lambda to perform Process 1
            Function process11Function = new Function(this, "Process11Function", new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset("src/Demo.Services.Lambda/bin/Release/netcoreapp3.1/Demo.Services.Lambda.zip"),
                Handler = "Demo.Services.Lambda::Demo.Services.Lambda.Functions::Process11Function",
                Timeout = Duration.Minutes(5),
                MemorySize = 512,
                Description = "Demo Lambda Function that runs job process1",

            });

            //Demo Lambda to perform Process 2
            Function process12Function = new Function(this, "Process12Function", new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset("src/Demo.Services.Lambda/bin/Release/netcoreapp3.1/Demo.Services.Lambda.zip"),
                Handler = "Demo.Services.Lambda::Demo.Services.Lambda.Functions::Process12Function",
                Timeout = Duration.Minutes(5),
                MemorySize = 512,
                Description = "Demo Lambda Function that runs job process2",

            });
/*
            //State Machines that operates on odd or even logic
            //lambda Invoke that calculates odd or even
            //var startStep = new LambdaInvoke(this, "Start", new LambdaInvokeProps
            //{
            //    LambdaFunction = oddOrEvenFunction.LatestVersion,

            //});

            var oddEvenFunction = new Task(this, "OddEvenFunction", new TaskProps
            {
                Task = new InvokeFunction(oddOrEvenFunction.LatestVersion)
            });

            //lambda Invoke that does Process 1
            //var process1 = new LambdaInvoke(this, "Process1", new LambdaInvokeProps
            //{
            //    LambdaFunction = process1Function.LatestVersion,
            //});

            var process1 = new Task(this, "Process1", new TaskProps
            {
                Task = new InvokeFunction(process1Function.LatestVersion)
            });

            //lambda Invoke that does Process 2
            //var process2 = new LambdaInvoke(this, "Process2", new LambdaInvokeProps
            //{
            //    LambdaFunction = process2Function.LatestVersion,
            //});

            var process2 = new Task(this, "Process2", new TaskProps
            {
                Task = new InvokeFunction(process2Function.LatestVersion)
            });

            //Choice to go to Process 1 or Process 2 based on input number is odd or even.
            var isEven = new Choice(this, "Is the number Even?");

            var chain = Chain.Start(oddEvenFunction).Next(
                isEven
                    .When(Condition.StringEquals("$.Result", "Even"), process1)
                    .When(Condition.StringEquals("$.Result", "Even"), process2)
            );

*/
            //State Machine
            /*
            var stateMachine = new StateMachine(this, "DemoStepFunction", new StateMachineProps
            {
                StateMachineName = "Demo-State-Machine",
                Timeout = Duration.Minutes(5),
                Definition = chain
            });
            */



            //All Policies
            // 1. Invoke function policies
            /*
            invokeOddEvenStepFunction.Role?.AddManagedPolicy(ManagedPolicy.FromManagedPolicyArn(this, "InvokeLambdaPolicy", "arn:aws:iam::aws:policy/AWSLambdaFullAccess"));
            var policyStatement = new PolicyStatement
            {
                Sid = "CanInvokeStepFunctions",
                Effect = Effect.ALLOW
            };
            policyStatement.AddActions(new[] { "states:StartExecution" });

            invokeOddEvenStepFunction.AddToRolePolicy(policyStatement);
            policyStatement.AddResources(stateMachine.StateMachineArn);
            invokeOddEvenStepFunction.AddEnvironment(Functions.OddEvenStateMachineArnKey, stateMachine.StateMachineArn);
            */
            process12Function.AddEnvironment(Functions.StepFunctionDemoBucketKey, stepFunctionDemoBucket.BucketName);
            stepFunctionDemoBucket.GrantReadWrite(process12Function);
        }

    }
}
