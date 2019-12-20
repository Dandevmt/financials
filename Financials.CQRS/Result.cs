using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.CQRS
{
    public class Result
    {
        public bool IsSuccess { get; protected set; }
        public IList<IError> Errors { get; protected set; }

        protected Result()
        {
            Errors = new List<IError>();
        }

        public static Result Success()
        {
            return new Result() { IsSuccess = true };
        }

        public static Result Fail(IError error)
        {
            return new Result() { IsSuccess = false, Errors = new List<IError> { error } };
        }

        public void AddError(IError error)
        {
            Errors.Add(error);
        }

        public Result Merge(Result result)
        {
            if (!result.IsSuccess)
            {
                IsSuccess = false;
                foreach(var error in result.Errors)
                {
                    Errors.Add(error);
                }
            }
            return this;
        }

        public Result Merge(params Result[] results)
        {
            foreach(var result in results)
            {
                Merge(result);
            }
            return this;
        }

        public static Result operator +(Result resulta, Result resultb)
        {
            return resulta.Merge(resultb);
        }


    }

    public class Result<T> : Result
    {
        public T Value { get; protected set; }

        public static Result<T> Success(T value)
        {
            return new Result<T>()
            {
                IsSuccess = true,
                Value = value
            };
        }

        public static Result<T> Fail(T value, IError error)
        {            
            return new Result<T>()
            {
                IsSuccess = false,
                Errors = new List<IError> { error },
                Value = value
            };
        }

        public new Result<T> Merge(Result result)
        {
            if (!result.IsSuccess)
            {
                IsSuccess = false;
                foreach (var error in result.Errors)
                {
                    Errors.Add(error);
                }
            }
            return this;
        }

        public new Result<T> Merge(params Result[] results)
        {
            foreach (var result in results)
            {
                Merge(result);
            }
            return this;
        }

        public static Result<T> operator +(Result<T> resulta, Result resultb)
        {
            return resulta.Merge(resultb);
        }
    }
}
