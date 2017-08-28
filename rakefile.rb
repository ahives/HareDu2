require 'azdeploy'

COPYRIGHT = "Copyright 2013-2017 Albert L. Hives"
PRODUCT = "HareDu"
PROJECT_URL = "http://github.com/ahives/HareDu2"
CLR_TOOLS_VERSION = 'v4.0.30319'
OUTPUT_PATH = 'bin/Release'
NUGET = 'src/.nuget/nuget.exe'

build_paths = {
    :src => File.expand_path('src'),
    :output => File.expand_path('build_output'),
    :artifacts => File.expand_path('build_artifacts'),
    :projects => [PRODUCT],
}

outputs = {
    "#{PRODUCT}/bin/Release" => "#{PRODUCT}.{dll,pdb,xml}"
}

supporting_output_dlls = {
    "#{PRODUCT}/bin/Release" => "#{PRODUCT}.{dll,pdb,xml}"
}

desc '**Default**, compiles and runs tests'
task :default => [:clean, :restore, :compile, :package, :publish]

desc 'Prepares the working directory for a new build'
task :clean do
    cleantask(build_paths)
end

desc 'restores missing packages'
task :restore do
    restore_nuget(NUGET)
end

desc 'Cleans, versions, compiles the application and generates build_output.'
task :compile => [:versioning, :global_version, :build, :tests, :copy_output]

desc 'Only compiles the application.'
msbuild :build do |msb|
    msb.targets :Clean, :Build
    clean_build(msb, "src/#{PRODUCT}.sln")
end

desc "Publishes."
msbuild :publish do |msb|
    msb.targets :Publish
    clean_build(msb, "src/#{PRODUCT}.sln")
end

desc 'Copies build output to designated folders.'
task :copy_output do
    outputs.each { |name, filter|
        copy_output_files File.join(build_paths[:src], name), filter, File.join(build_paths[:output], 'net-4.5')
    }
end

desc 'Copies supporting dlls to designated folders.'
task :copy_output do
    supporting_output_dlls.each { |name, filter|
        copy_output_files File.join(build_paths[:src], name), filter, File.join(build_paths[:output], 'net-4.5')
    }
end

desc 'Copies runtime to artifacts folder.'
task :copy_runtime do
    runtime.each { |name, path|
        copy_output_files File.join(build_paths[:src], path), "*.*", File.join(build_paths[:artifacts], name)
    }
end

desc 'Update the common version information for the build. You can call this task without building.'
assemblyinfo :global_version => [:versioning] do |asm|
    set_framework_version(asm)
end

desc 'Update the common version information for the build. You can call this task without building.'
assemblyinfo :global_version => [:versioning] do |asm|
    set_solution_version(asm)
end

desc 'Versions current build'
task :versioning do
    versioning
end

desc "Builds the nuget package"
task :package => [:versioning, :create_nuspec] do
    sh "#{NUGET} pack #{build_paths[:artifacts]}/#{PRODUCT}.nuspec /OutputDirectory #{build_paths[:artifacts]}"
end

task :create_nuspec => [:haredu_nuspec]

LANG = 'en-US'
SRC_FOLDER = 'src'
FALSE_STR = 'false'

nuspec :haredu_nuspec do |nuspec|
    nuspec.id = "#{PRODUCT}"
    nuspec.version = ENV['NUGET_VERSION']
    nuspec.authors = "Albert L. Hives"
    nuspec.description = 'HareDu is a .NET API that can be used to manage and monitor a RabbitMQ server or cluster.'
    nuspec.title = "#{PRODUCT}"
    nuspec.projectUrl = PROJECT_URL
    nuspec.language = LANG
    nuspec.licenseUrl = "http://www.apache.org/licenses/LICENSE-2.0"
    nuspec.requireLicenseAcceptance = FALSE_STR
    nuspec.dependency "Common.Logging", "3.3.1"
    nuspec.dependency "Microsoft.AspNet.WebApi.Client", "5.2.3"
    nuspec.dependency "Microsoft.Net.Http", "4.3.2"
    nuspec.dependency "Newtonsoft.Json", "10.0.3"
    nuspec.dependency "Polly-Signed", "5.3.0"
    nuspec.output_file = File.join(build_paths[:artifacts], "#{PRODUCT}.nuspec")
    add_files build_paths[:output], "#{PRODUCT}.{dll,pdb,xml}", nuspec
end
