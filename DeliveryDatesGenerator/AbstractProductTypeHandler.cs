using DeliveryDatesGenerator.Models;

namespace DeliveryDatesGenerator
{
   public abstract class AbstractProductTypeHandler : IHandler
    {
        private IHandler? nextHandler;

        /// <summary>
        /// Returning a handler from here will let us link handlers in a
        /// convenient way like this:
        /// Normal.SetNext(External).SetNext(Temporary);
        /// </summary>
        /// <param name="handler"></param>
        /// <returns>IHandler</returns>
        public IHandler SetNext(IHandler handler)
        {
            this.nextHandler = handler;
            return handler;
        }
        public virtual int? Handle(Product product)
        {
            if (this.nextHandler != null)
            {
                return this.nextHandler.Handle(product);
            }
            else
            {
                return null;
            }
        }
    }

}