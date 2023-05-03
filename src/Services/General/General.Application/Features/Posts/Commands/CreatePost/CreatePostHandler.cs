using General.Application.Contracts;
using General.Domain.Entities;
using General.Domain.ValueObjects;
using Shared.Implementations.Abstractions;
using Shared.Implementations.FileOperations;
using Shared.Implementations.Services;
using Shared.Implementations.Tools;

namespace General.Application.Features.Posts.Commands.CreatePost;

public class CreatePostHandler : ICommandHandler<CreatePostCommand, int>
{
    private readonly IPostRepository _postRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IFileService _fileService;

    public CreatePostHandler(IPostRepository postRepository, ICurrentUser currentUser, IFileService fileService)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
    }
    public async Task<int> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var organization = request.IsPublic ? null : _currentUser.OrganizationCode;
        var dictionaryFiles = request.Attachments?.ToDictionary(_ => _.CreateFilePath(), _ => _);
        var attachments = dictionaryFiles?.Select(_ => Attachment.Create(_.Key, request.Path)).ToList();
        var newPost = Post.Create(_currentUser.UserId, request.Description, request.IsPublic, organization, attachments);

        await _postRepository.AddAsync(newPost);
         
        var paths = new List<string>();
        try
        {
            if (dictionaryFiles != null && dictionaryFiles.Any())
            {
                foreach (var keyValuePair in dictionaryFiles)
                {
                    if (keyValuePair.Value.Length > 0)
                    {
                        var path = await _fileService.SaveFileAsync(keyValuePair.Value, request.Path, keyValuePair.Key);
                        paths.Add(path);
                    }
                }
            }

            await _postRepository.SaveAsync();
        }
        catch
        {
            if (paths.Any())
            {
                foreach (var path in paths)
                {
                    _fileService.RemoveFile(path);
                }
            }

            throw;
        }

        return newPost.Id;
    }
}