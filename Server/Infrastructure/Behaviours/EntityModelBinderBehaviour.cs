using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContosoUniversity.Shared.Domain.SeedWork;
using ContosoUniversity.Shared.Domain.UniversityAggregate;
using ContosoUniversity.Shared.Infrastructure;
using ContosoUniversity.Shared.Infrastructure.ModelBinding;
using MediatR;

namespace ContosoUniversity.Server.Infrastructure.Behaviours
{
    public class EntityModelBinderBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest,TResponse>
    {
        private readonly SchoolContext _context;
        public EntityModelBinderBehaviour(SchoolContext context)
        {
            _context = context;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var properties = typeof(TRequest).GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(BindRequestProperty))).ToArray();

            if (!properties.Any())
            {
                return await next();
            }

            foreach (var property in properties)
            {
                var obj = property.GetValue(request) as Entity;
                if(obj == null)
                {
                    continue;
                }

                var entity = obj switch
                {
                    Course _ => (Entity)await _context.Set<Course>().FindAsync(obj.Id),
                    Department _ => await _context.Set<Department>().FindAsync(obj.Id),
                    Enrollment _ => await _context.Set<Enrollment>().FindAsync(obj.Id),
                    Instructor _ => await _context.Set<Instructor>().FindAsync(obj.Id),
                    Student _ => await _context.Set<Student>().FindAsync(obj.Id),
                    _ => null
                };

                property.SetValue(request, entity);
            }

            return await next();
        }
    }
}
