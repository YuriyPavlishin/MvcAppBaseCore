using System;
using System.Linq.Expressions;
using Moq;

namespace BaseApp.Tests.Utils;

public static class UnitOfWorkTestExtensions
{
    public static Mock<TR> MockRepository<TR, TH>(this Mock<TH> holder, Expression<Func<TH, TR>> repExpression) 
        where TR: class where TH: class
    {
        var userValidatorMock = new Mock<TR>();
        holder.SetupGet(repExpression).Returns(userValidatorMock.Object);
        return userValidatorMock;
    }
}